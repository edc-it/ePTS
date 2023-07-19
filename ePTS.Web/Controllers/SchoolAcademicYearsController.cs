using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Core;
using Microsoft.AspNetCore.Identity;
using ePTS.Entities.Identity;
using ePTS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class SchoolAcademicYearsController : BaseController
    {

        public SchoolAcademicYearsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AssessmentResultsController> logger) : base(context, logger, userManager)
        {
        }

        // GET: SchoolAcademicYears
        public async Task<IActionResult> Index(Guid? id)
        {
            // id is SchoolId
            if (id == null)
            {
                return NotFound();
            }

            ViewData["NextController"] = "Gradebooks";
            ViewData["ParentController"] = "Schools";
            ViewData["ParentId"] = id;

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);




            var organization = await _context.Schools
                .Include(s => s.Locations)
                .Include(s => s.OrganizationTypes)
                .Include(s => s.ParentOrganizations)
                .Include(s => s.SchoolAdministrationTypes)
                .Include(s => s.SchoolLanguages)
                .Include(s => s.SchoolLocations)
                .Include(s => s.SchoolStatus)
                .Include(s => s.SchoolTypes)
                .Where(x => x.OrganizationId == id)
                .FirstOrDefaultAsync();

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
                CurrentLevel = 5,
                CurrentController = "SchoolAcademicYears",
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationType = organization.OrganizationTypes!.OrganizationType,
                Code = organization.Code,
                IsOrganizationUnit = organization.IsOrganizationUnit,
                OrganizationParent = organizationHierarchy, //organizationParents,
                LocationParent = organizationLocations,
                UserOrganizationId = userOrganizationIds.Contains(organization.OrganizationId) ? organization.OrganizationId : null,
                Zone = organization.ParentOrganizations == null ? null : organization.ParentOrganizations.OrganizationName,
                Address = organization.Address,
                Latitude = organization.Latitude,
                Longitude = organization.Longitude,
                //EMIS = organization.SchoolCode,
                SchoolType = organization.SchoolTypes == null ? null : organization.SchoolTypes.SchoolType,
                SchoolLocation = organization.SchoolLocations == null ? null : organization.SchoolLocations.SchoolLocation,
                SchoolAdministrationType = organization.SchoolAdministrationTypes == null ? null : organization.SchoolAdministrationTypes.SchoolAdministrationType,
                SchoolLanguage = organization.SchoolLanguages == null ? null : organization.SchoolLanguages.SchoolLanguage,
                ParentOrganizationId = organization.ParentOrganizations == null ? null : organization.ParentOrganizations.OrganizationId,
            };
            
            var schoolAcademicYears = await _context.SchoolAcademicYears
                .Where(x => x.OrganizationId == id)
                .Select(x => new SchoolAcademicYearViewModel
                {
                    SchoolAcademicYearId = x.SchoolAcademicYearId,
                    AcademicYear = x.AcademicYears!.AcademicYear,
                    AcademicYearStatus = x.AcademicYearStatus!.AcademicYearStatus,
                    RegistrationDate = x.RegistrationDate,
                    SortOrder = x.AcademicYears!.SortOrder,
                    GradebookCount = x.Gradebooks!.Count(),
                    //IsCurrent = x.IsCurrent
                })
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
            
            var model = new SchoolAcademicYearSummaryViewModel
            {
                InfoPanel = infoPanel,
                SchoolAcademicYears = schoolAcademicYears
            };

            return View(model);

        }

        // GET: SchoolAcademicYears/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SchoolAcademicYears == null)
            {
                return NotFound();
            }

            var schoolAcademicYear = await _context.SchoolAcademicYears
                .Include(s => s.AcademicYearStatus)
                .Include(s => s.AcademicYears)
                .Include(s => s.Schools)
                .FirstOrDefaultAsync(m => m.SchoolAcademicYearId == id);
            if (schoolAcademicYear == null)
            {
                return NotFound();
            }

            return View(schoolAcademicYear);
        }

        // GET: SchoolAcademicYears/Create
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var existingAcademicYears = await _context.SchoolAcademicYears
                .Where(x => x.OrganizationId == id)
                .Select(x => x.RefAcademicYearId)
                .ToListAsync();

            //return view data for ViewData["RefAcademicYearId"] where RefAcademicYearId is not in existingAcademicYears
            ViewData["RefAcademicYearId"] = new SelectList(_context.AcademicYears
                .Where(x => !existingAcademicYears.Contains(x.RefAcademicYearId)), "RefAcademicYearId", "AcademicYear");
            
            ViewData["ParentId"] = id;

            return View();
        }

        // POST: SchoolAcademicYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolAcademicYearId,OrganizationId,RefAcademicYearId,RegistrationDate,StartDate,EndDate,RefAcademicYearStatusId,IsMissingEnrollment")] SchoolAcademicYear schoolAcademicYear)
        {
            if (ModelState.IsValid)
            {
                schoolAcademicYear.SchoolAcademicYearId = Guid.NewGuid();
                _context.Add(schoolAcademicYear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new {id = schoolAcademicYear.OrganizationId});
            }
            ViewData["RefAcademicYearStatusId"] = new SelectList(_context.AcademicYearStatus, "RefAcademicYearStatusId", "AcademicYearStatus", schoolAcademicYear.RefAcademicYearStatusId);
            ViewData["RefAcademicYearId"] = new SelectList(_context.AcademicYears, "RefAcademicYearId", "AcademicYear", schoolAcademicYear.RefAcademicYearId);
            ViewData["OrganizationId"] = new SelectList(_context.Schools, "OrganizationId", "Code", schoolAcademicYear.OrganizationId);
            return View(schoolAcademicYear);
        }

        // GET: SchoolAcademicYears/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.SchoolAcademicYears == null)
            {
                return NotFound();
            }

            var schoolAcademicYear = await _context.SchoolAcademicYears.FindAsync(id);
            if (schoolAcademicYear == null)
            {
                return NotFound();
            }

            var existingAcademicYears = await _context.SchoolAcademicYears
                .Where(x => x.OrganizationId == schoolAcademicYear.OrganizationId)
                .Select(x => x.RefAcademicYearId)
                .ToListAsync();

            //return view data for ViewData["RefAcademicYearId"] where RefAcademicYearId is not in existingAcademicYears
            ViewData["RefAcademicYearId"] = new SelectList(_context.AcademicYears
                .Where(x => x.RefAcademicYearId == schoolAcademicYear.RefAcademicYearId || !existingAcademicYears.Contains(x.RefAcademicYearId)), "RefAcademicYearId", "AcademicYear", schoolAcademicYear.RefAcademicYearId);

            ViewData["ParentId"] = id;
            
            ViewData["OrganizationId"] = schoolAcademicYear.OrganizationId;
            return View(schoolAcademicYear);
        }

        // POST: SchoolAcademicYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SchoolAcademicYearId,OrganizationId,RefAcademicYearId,RegistrationDate,StartDate,EndDate,RefAcademicYearStatusId,IsMissingEnrollment")] SchoolAcademicYear schoolAcademicYear)
        {
            if (id != schoolAcademicYear.SchoolAcademicYearId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolAcademicYear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolAcademicYearExists(schoolAcademicYear.SchoolAcademicYearId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = schoolAcademicYear.OrganizationId });
            }
            ViewData["RefAcademicYearStatusId"] = new SelectList(_context.AcademicYearStatus, "RefAcademicYearStatusId", "AcademicYearStatus", schoolAcademicYear.RefAcademicYearStatusId);
            ViewData["RefAcademicYearId"] = new SelectList(_context.AcademicYears, "RefAcademicYearId", "AcademicYear", schoolAcademicYear.RefAcademicYearId);
            ViewData["OrganizationId"] = new SelectList(_context.Schools, "OrganizationId", "Code", schoolAcademicYear.OrganizationId);
            return View(schoolAcademicYear);
        }

        // GET: SchoolAcademicYears/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SchoolAcademicYears == null)
            {
                return NotFound();
            }

            var schoolAcademicYear = await _context.SchoolAcademicYears
                .Include(s => s.AcademicYearStatus)
                .Include(s => s.AcademicYears)
                .Include(s => s.Schools)
                .FirstOrDefaultAsync(m => m.SchoolAcademicYearId == id);
            if (schoolAcademicYear == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = schoolAcademicYear.OrganizationId;

            return View(schoolAcademicYear);
        }

        // POST: SchoolAcademicYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.SchoolAcademicYears == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SchoolAcademicYears'  is null.");
            }
            var schoolAcademicYear = await _context.SchoolAcademicYears.FindAsync(id);
            if (schoolAcademicYear != null)
            {
                _context.SchoolAcademicYears.Remove(schoolAcademicYear);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = schoolAcademicYear.OrganizationId });
        }

        private bool SchoolAcademicYearExists(Guid id)
        {
          return (_context.SchoolAcademicYears?.Any(e => e.SchoolAcademicYearId == id)).GetValueOrDefault();
        }
    }
}
