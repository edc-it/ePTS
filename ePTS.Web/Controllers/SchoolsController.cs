using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Core;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using ePTS.Entities.Gradebooks;
using ePTS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ePTS.Entities.Reference;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class SchoolsController : BaseController
    {

        public SchoolsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrganizationsController> logger) : base(context, logger, userManager)
        {
        }

        // GET: Schools
        public async Task<IActionResult> Index(Guid? id)
        {
            ViewData["NextController"] = "SchoolAcademicYears";
            ViewData["ParentController"] = "Organizations";
            ViewData["ParentId"] = id;

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);

            var organization = _context.Organizations.Find(id);

            if (organization == null)
            {
                return NotFound();
            }

            var organizationHierarchy = id.HasValue
                ? await GetBreadcrumbAsync(id.Value)
                : new List<Organization>();

            // Get Parent Organization hierarchy
            IEnumerable<OrganizationParentViewModel>? organizationParents = new List<OrganizationParentViewModel>();
            IEnumerable<LocationParentViewModel>? organizationLocations = new List<LocationParentViewModel>();

            // Get Parent Location hierarchy
            if (organization.RefLocationId != null)
            {
                organizationLocations = (IEnumerable<LocationParentViewModel>)await GetLocationHierarchyAsync(organization.RefLocationId);
            }
            else
            {
                organizationLocations = null;
            }

            if (organization.OrganizationId != Guid.Empty)
            {
                organizationParents = (IEnumerable<OrganizationParentViewModel>)await GetParentHierarchyAsync(organization.OrganizationId);
            }
            else
            {
                organizationParents = null;
            }

            var infoPanel = new InfoPanelViewModel
            {
                CurrentLevel = 4,
                CurrentController = "Schools",
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationType = organization.OrganizationTypes == null ? null : organization.OrganizationTypes.OrganizationType,
                Code = organization.Code,
                IsOrganizationUnit = organization.IsOrganizationUnit,
                OrganizationParent = organizationHierarchy, //organizationParents,
                LocationParent = organizationLocations,
                UserOrganizationId = userOrganizationIds.Contains(organization.OrganizationId) ? organization.OrganizationId : null,
                ParentOrganizationId = organization.ParentOrganizationId,

            };

            var schools = await _context.Schools
                .Include(s => s.Locations)
                .Include(s => s.OrganizationTypes)
                .Include(s => s.ParentOrganizations)
                .Include(s => s.SchoolAdministrationTypes)
                .Include(s => s.SchoolLanguages)
                .Include(s => s.SchoolLocations)
                .Include(s => s.SchoolStatus)
                .Include(s => s.SchoolTypes)
                .Where(s => s.ParentOrganizationId == id && childOrganizationIds.Contains(s.OrganizationId))
                .OrderBy(s => s.OrganizationName)
                .ToListAsync();

            var model = new SchoolSummaryViewModel
            {
                InfoPanel = infoPanel,
                Schools = schools
            };

            return View(model);
        }

        // GET: Schools/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Schools == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .Include(s => s.Locations)
                .Include(s => s.OrganizationTypes)
                .Include(s => s.ParentOrganizations)
                .Include(s => s.SchoolAdministrationTypes)
                .Include(s => s.SchoolLanguages)
                .Include(s => s.SchoolLocations)
                .Include(s => s.SchoolStatus)
                .Include(s => s.SchoolTypes)
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // GET: Schools/Create
        public IActionResult Create(Guid? id)
        {

            ViewData["ParentId"] = id;

            var locationTypes = _context.LocationTypes.Where(x => x.LocationLevel != 1);
            ViewData["RefLocationTypes"] = locationTypes;
            ViewData["RefLocationTypesCount"] = locationTypes.Count();
            ViewData["RefOrganizationTypeId"] = _context.OrganizationTypes.Where(x => x.IsSchool == true).Select(x => x.RefOrganizationTypeId).FirstOrDefault();

            ViewData["RefLocationId"] = new SelectList(_context
                .Locations
                .Include(x => x.LocationTypes)
                .Where(x =>
                    x.LocationTypes!.LocationLevel == 2 )
                .Select(x => new
                {
                    x.RefLocationId,
                    x.LocationName
                }), "RefLocationId", "LocationName");

            //ViewData["RefLocationId"] = new SelectList(_context.Locations, "RefLocationId", "RefLocationId");
            //ViewData["RefOrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "RefOrganizationTypeId", "OrganizationType");
            ViewData["ParentOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Code");
            ViewData["RefSchoolAdministrationTypeId"] = new SelectList(_context.SchoolAdministrationTypes, "RefSchoolAdministrationTypeId", "SchoolAdministrationType");
            ViewData["RefSchoolLanguageId"] = new SelectList(_context.SchoolLanguages, "RefSchoolLanguageId", "SchoolLanguage");
            ViewData["RefSchoolLocationId"] = new SelectList(_context.SchoolLocations, "RefSchoolLocationId", "SchoolLocation");
            ViewData["RefSchoolStatusId"] = new SelectList(_context.SchoolStatus, "RefSchoolStatusId", "SchoolStatus");
            ViewData["RefSchoolTypeId"] = new SelectList(_context.SchoolTypes, "RefSchoolTypeId", "SchoolType");
            return View();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolCode,RefSchoolTypeId,RefSchoolLocationId,RefSchoolAdministrationTypeId,RefSchoolLanguageId,RefSchoolStatusId,HeadTeacher,OpeningDate,ClosingDate,OrganizationId,RegistrationDate,Code,OrganizationName,RefOrganizationTypeId,RefLocationId,Address,ParentOrganizationId,Latitude,Longitude,IsOrganizationUnit")] School school, string[] RefLocationId)
        {
            if (ModelState.IsValid)
            {
                school.OrganizationId = Guid.NewGuid();
                // Gets the last non-null item in RefLocationId array,
                // the array might have null values for locations left empty
                // and this assigns the last non-null location to RefLocationId
                if (RefLocationId.Length != 0)
                {
                    var location =
                    (from r in RefLocationId where !string.IsNullOrEmpty(r) select r)
                    .OrderByDescending(r => r).Count();

                    school.RefLocationId = RefLocationId[location - 1];
                }
                _context.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = school.ParentOrganizationId });
            }
            ViewData["RefLocationId"] = new SelectList(_context.Locations, "RefLocationId", "RefLocationId", school.RefLocationId);
            ViewData["RefOrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "RefOrganizationTypeId", "OrganizationType", school.RefOrganizationTypeId);
            ViewData["ParentOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Code", school.ParentOrganizationId);
            ViewData["RefSchoolAdministrationTypeId"] = new SelectList(_context.SchoolAdministrationTypes, "RefSchoolAdministrationTypeId", "SchoolAdministrationType", school.RefSchoolAdministrationTypeId);
            ViewData["RefSchoolLanguageId"] = new SelectList(_context.SchoolLanguages, "RefSchoolLanguageId", "SchoolLanguage", school.RefSchoolLanguageId);
            ViewData["RefSchoolLocationId"] = new SelectList(_context.SchoolLocations, "RefSchoolLocationId", "SchoolLocation", school.RefSchoolLocationId);
            ViewData["RefSchoolStatusId"] = new SelectList(_context.SchoolStatus, "RefSchoolStatusId", "SchoolStatus", school.RefSchoolStatusId);
            ViewData["RefSchoolTypeId"] = new SelectList(_context.SchoolTypes, "RefSchoolTypeId", "SchoolType", school.RefSchoolTypeId);
            return View(school);
        }

        // GET: Schools/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Schools == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .Include(x => x.ParentOrganizations)
                .Where(x => x.OrganizationId == id)
                .FirstOrDefaultAsync();


            if (school == null)
            {
                return NotFound();
            }

            var locationTypes = _context.LocationTypes.Where(x => x.LocationLevel != 1);
            ViewData["RefLocationTypes"] = locationTypes;
            ViewData["RefLocationTypesCount"] = locationTypes.Count();
            //ViewData["RefOrganizationTypeId"] = _context.OrganizationTypes.Where(x => x.IsSchool == true).Select(x => x.RefOrganizationTypeId).FirstOrDefault();

            ViewData["RefLocationId"] = new SelectList(_context
                .Locations
                .Include(x => x.LocationTypes)
                .Where(x =>
                    x.LocationTypes!.LocationLevel == 2)
                .Select(x => new
                {
                    x.RefLocationId,
                    x.LocationName
                }), "RefLocationId", "LocationName", school.RefLocationId);

            //Get Location Parents (only the lower level RefLocationId is saved, 
            //this gets the location parents for the saved location
            IEnumerable<LocationParentViewModel>? organizationLocations = new List<LocationParentViewModel>();

            // Get Parent Location hierarchy
            if (school.RefLocationId != null)
            {
                organizationLocations = (IEnumerable<LocationParentViewModel>)await GetLocationHierarchyAsync(school.RefLocationId);
            }
            else
            {
                organizationLocations = null;
            }
            var allLocations = _context.Locations;

            ViewData["ParentId"] = school.ParentOrganizationId;
            ViewData["RefOrganizationTypeId"] = school.RefOrganizationTypeId;
            ViewData["ParentOrganizationId"] = school.ParentOrganizationId;
            //var locations = ListLocations(allLocations, organization.RefLocationId!);
            ViewData["RefLocationParents"] = organizationLocations;


            ViewData["RefSchoolAdministrationTypeId"] = new SelectList(_context.SchoolAdministrationTypes, "RefSchoolAdministrationTypeId", "SchoolAdministrationType", school.RefSchoolAdministrationTypeId);
            ViewData["RefSchoolLanguageId"] = new SelectList(_context.SchoolLanguages, "RefSchoolLanguageId", "SchoolLanguage", school.RefSchoolLanguageId);
            ViewData["RefSchoolLocationId"] = new SelectList(_context.SchoolLocations, "RefSchoolLocationId", "SchoolLocation", school.RefSchoolLocationId);
            ViewData["RefSchoolStatusId"] = new SelectList(_context.SchoolStatus, "RefSchoolStatusId", "SchoolStatus", school.RefSchoolStatusId);
            ViewData["RefSchoolTypeId"] = new SelectList(_context.SchoolTypes, "RefSchoolTypeId", "SchoolType", school.RefSchoolTypeId);

            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SchoolCode,RefSchoolTypeId,RefSchoolLocationId,RefSchoolAdministrationTypeId,RefSchoolLanguageId,RefSchoolStatusId,HeadTeacher,OpeningDate,ClosingDate,OrganizationId,RegistrationDate,Code,OrganizationName,RefOrganizationTypeId,RefLocationId,Address,ParentOrganizationId,Latitude,Longitude,IsOrganizationUnit")] School school, string[] RefLocationId)
        {
            if (id != school.OrganizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Gets the last non-null item in RefLocationId array,
                    // the array might have null values for locations left empty
                    // and this assigns the last non-null location to RefLocationId
                    if (RefLocationId.Length != 0)
                    {
                        var location =
                        (from r in RefLocationId where !string.IsNullOrEmpty(r) select r)
                        .OrderByDescending(r => r).Count();

                        school.RefLocationId = RefLocationId[location - 1];
                    }
                    _context.Update(school);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolExists(school.OrganizationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { id = school.ParentOrganizationId});
            }
            ViewData["RefLocationId"] = new SelectList(_context.Locations, "RefLocationId", "RefLocationId", school.RefLocationId);
            ViewData["RefOrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "RefOrganizationTypeId", "OrganizationType", school.RefOrganizationTypeId);
            ViewData["ParentOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Code", school.ParentOrganizationId);
            ViewData["RefSchoolAdministrationTypeId"] = new SelectList(_context.SchoolAdministrationTypes, "RefSchoolAdministrationTypeId", "SchoolAdministrationType", school.RefSchoolAdministrationTypeId);
            ViewData["RefSchoolLanguageId"] = new SelectList(_context.SchoolLanguages, "RefSchoolLanguageId", "SchoolLanguage", school.RefSchoolLanguageId);
            ViewData["RefSchoolLocationId"] = new SelectList(_context.SchoolLocations, "RefSchoolLocationId", "SchoolLocation", school.RefSchoolLocationId);
            ViewData["RefSchoolStatusId"] = new SelectList(_context.SchoolStatus, "RefSchoolStatusId", "SchoolStatus", school.RefSchoolStatusId);
            ViewData["RefSchoolTypeId"] = new SelectList(_context.SchoolTypes, "RefSchoolTypeId", "SchoolType", school.RefSchoolTypeId);
            return View(school);
        }

        // GET: Schools/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Schools == null)
            {
                return NotFound();
            }

            var school = await _context.Schools
                .Include(s => s.Locations)
                .Include(s => s.OrganizationTypes)
                .Include(s => s.ParentOrganizations)
                .Include(s => s.SchoolAdministrationTypes)
                .Include(s => s.SchoolLanguages)
                .Include(s => s.SchoolLocations)
                .Include(s => s.SchoolStatus)
                .Include(s => s.SchoolTypes)
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (school == null)
            {
                return NotFound();
            }

            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Schools == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Schools'  is null.");
            }
            var school = await _context.Schools.FindAsync(id);
            if (school != null)
            {
                _context.Schools.Remove(school);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolExists(Guid id)
        {
          return (_context.Schools?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }

        public class OrganizationWithSchools
        {
            public Organization? Organization { get; set; }
            public List<School>? Schools { get; set; }
        }
    }
}
