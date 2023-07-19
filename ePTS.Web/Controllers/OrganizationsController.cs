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
using ePTS.Web.Extensions;
using ePTS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ePTS.Entities.Reference;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class OrganizationsController : BaseController
    {

        public OrganizationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrganizationsController> logger) : base(context, logger, userManager)
        {
        }

        // GET: Organizations

        public async Task<IActionResult> SelectOrganization()
        {
            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);

            var organizations = await _context.Organizations
                .Where(o => userOrganizationIds.Contains(o.OrganizationId))
                .Select(o => new OrganizationSelectViewModel
                {
                    OrganizationId = o.OrganizationId,
                    OrganizationName = o.OrganizationName,
                    Code = o.Code,
                    OrganizationType = o.OrganizationTypes!.OrganizationType,
                    IsOrganizationUnit = o.IsOrganizationUnit

                })
                .ToListAsync();

            // If the user is assigned to only one organization, select it automatically and redirect
            if (organizations.Count == 1)
            {
                HttpContext.Session.SetString("SelectedOrganizationId", organizations[0].OrganizationId.ToString());

                // Get the session value
                var sessionValue = HttpContext.Session.GetString("SelectedOrganizationId");
                Console.WriteLine($"Session Value: {sessionValue}"); // Check the console for this output.


                return RedirectToAction("Index");
            }

            return View(organizations);
        }

        [HttpPost]
        public IActionResult SelectOrganization(Guid organizationId)
        {
            HttpContext.Session.SetString("SelectedOrganizationId", organizationId.ToString());

            // Get the session value
            var sessionValue = HttpContext.Session.GetString("SelectedOrganizationId");
            Console.WriteLine($"Session Value: {sessionValue}"); // Check the console for this output.

            return RedirectToAction("Index", new { id = organizationId }); // Redirect to wherever is appropriate after organization selection.
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            ViewData["NextController"] = "Schools";
            ViewData["ParentController"] = "Organizations";
            ViewData["ParentId"] = id;

            // Get the session value
            var selectedOrganizationId = await GetSelectedOrganization();

            // If the user is not assigned to any organization, redirect to the organization selection page
            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            ViewData["SelectedOrganizationId"] = selectedOrganizationId;

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);

            List<Organization> organizations = new();
            Organization? organization = null;

            if (id == null)
            {
                var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);
                organizations = await _context.Organizations
                    //.Where(o => userOrganizationIds.Contains(o.OrganizationId))
                    .Where(o => o.ParentOrganizationId == selectedOrganizationId && childOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();

                organization = await _context.Organizations
                .Include(x => x.OrganizationTypes)
                .Include(x => x.Locations)
                .Where(x => x.OrganizationId == selectedOrganizationId)
                .FirstOrDefaultAsync();
            }
            else
            {
                var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, id);
                organizations = await _context.Organizations
                    .Where(o => o.ParentOrganizationId == id && childOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();

                organization = await _context.Organizations
                .Include(x => x.OrganizationTypes)
                .Include(x => x.Locations)
                .Where(x => x.OrganizationId == id)
                .FirstOrDefaultAsync();
            }

            if (organization == null)
            {
                return NotFound();
            }

            //if the user organization is type school, redirect to the school controller
            if (organization.OrganizationTypes!.IsSchool == true)
            {
                return RedirectToAction("Index", "SchoolAcademicYears", new { id = organization.OrganizationId });
            }


            //TODO refactor this to use the organizationId instead of id
            var organizationHierarchy = id.HasValue
                ? await GetBreadcrumbAsync(id.Value)
                : new List<Organization>();

            if (organization != null) {

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
                    CurrentLevel = 0,
                    CurrentController = "Organizations",
                    OrganizationId = organization.OrganizationId,
                    OrganizationName = organization.OrganizationName,
                    OrganizationType = organization.OrganizationTypes!.OrganizationType,
                    Code = organization.Code,
                    IsOrganizationUnit = organization.IsOrganizationUnit,
                    OrganizationParent = organizationHierarchy, //organizationParents,
                    LocationParent = organizationLocations,
                    UserOrganizationId = userOrganizationIds.Contains(organization.OrganizationId) ? organization.OrganizationId : null,
                };

                var model = new OrganizationSummaryViewModel
                {
                    InfoPanel = infoPanel,
                    Organizations = organizations
                };

                return View(model);
            }
            else
            {
                var model = new OrganizationSummaryViewModel
                {
                    InfoPanel = null,
                    Organizations = organizations
                };

                return View(model);
            }
        }

        public async Task<IActionResult> IndexWorking(Guid? id)
        {
            ViewData["NextController"] = "SchoolAcademicYears";
            ViewData["ParentController"] = "Organizations";
            ViewData["ParentId"] = id;

                Guid? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

                var userOrganizationIds = _context.ApplicationUserOrganizations
                                .Where(x => x.UserId == userId)
                                .Select(x => x.OrganizationId).ToList();

            if (id == null)
            {
                // returns all organizations that are children of the user's organizations
                var query = await _context.Organizations
                    .Where(o => /*o.ParentOrganizationId == null && */ userOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();
                // returns all organizations that are children of the user's organizations

                return View(query);
            }
            else
            {
                var allOrganizations = _context.Organizations.ToList();

                // Convert list to lookup for fast in-memory queries
                var lookup = allOrganizations.ToLookup(x => x.ParentOrganizationId);

                // Use recursive method to get child organizations
                var childOrganizationIds = userOrganizationIds
                    .SelectMany(id => lookup[id].SelectRecursive(x => lookup[x.OrganizationId]))
                    .Select(x => x.OrganizationId)
                    .ToList();

                var query = await _context.Organizations
                    .Where(o => o.ParentOrganizationId == id && childOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();

                return View(query);
            }
            
        }

        public async Task<IActionResult> IndexOld(Guid? id)
        {
            ViewData["NextController"] = "SchoolAcademicYears";
            ViewData["ParentController"] = "Organizations";
            ViewData["ParentId"] = id;

            Guid? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            
            if(userId == null)
            {
                  return NotFound();
            }

            var userOrganizationIds = _context.ApplicationUserOrganizations
                .Where(x => x.UserId == userId)
                .Select(x => x.OrganizationId);

            if (userOrganizationIds == null)
            {
                return NotFound();
            }


            //var proficiencylevels = _context.AssessmentPerformanceLevels
            //    .Include(x => x.GradebookAssessments)
            //    .ThenInclude(x => x.Gradebooks)
            //    .ThenInclude(x => x.SchoolAcademicYears)
            //    .ThenInclude(x => x.Schools)
            //    .Include(x => x.PerformanceLevels)
            //    .Include(x => x.RefSex)
            //    //.Where(x => x.GradebookAssessments.Gradebooks.SchoolAcademicYears.OrganizationId == new Guid("6f493d8e-c402-4b9b-a0cb-003498760ada"))
            //    .Where(x => x.GradebookAssessments.Gradebooks.SchoolAcademicYears.OrganizationId == schoolacademicyear.Schools.OrganizationId)
            //    .GroupBy(x => new { x.PerformanceLevels.PerformanceLevel, x.RefSex.Sex })
            //    .Select(g => new
            //    {
            //        g.Key.PerformanceLevel,
            //        g.Key.Sex,
            //        Percent = g.Sum(x => x.PossibleValue) == 0 ? 0 : Math.Round(g.Sum(x => (decimal)x.ScoreValue) / g.Sum(x => (decimal)x.PossibleValue) * 100, 0)
            //    })
            //    .ToList();

            //ViewData["ProficiencyLevels"] = string.Join(", ", query.Select(q => q.Percent.ToString("F0")).ToArray());
            //original
            //if (id != null)
            //{
            //    // returns all organizations that are children of the selected organization
            //    var query = _context.Organizations
            //        .Where(x => x.OrganizationId == id)
            //        .Include(o => o.Locations)
            //        .Include(o => o.OrganizationTypes)
            //        .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes);
            //    _logger.LogInformation("Executing Index action1");
            //    return View(await query.FirstOrDefaultAsync());
            //}
            //else
            //{
            //    // returns all organizations that are children of the user's organizations
            //    var query = _context.Organizations
            //        .Where(o => userOrganizations.Contains(o.OrganizationId))
            //        .Include(o => o.Locations)
            //        .Include(o => o.OrganizationTypes)
            //        .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes);
            //    _logger.LogInformation("************** Executing Index action2");
            //    return View(await query.FirstOrDefaultAsync());
            //}

            if (id == null)
            {
                // returns all organizations that are children of the user's organizations
                var query = await _context.Organizations
                    .Where(o => /*o.ParentOrganizationId == null && */ userOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();
                // returns all organizations that are children of the user's organizations

                return View(query);
            }
            else
            {
                // returns all organizations that are children of the selected organization
                var childOrganizationIds = new List<Guid>();
                foreach (var userOrganizationId in userOrganizationIds)
                {
                    //var childIds = await GetChildOrganizationIds((Guid)userOrganizationId);
                    //childOrganizationIds.AddRange(childIds);
                }

                var query = await _context.Organizations
                    .Where(o => o.ParentOrganizationId == id &&  childOrganizationIds.Contains(o.OrganizationId))
                    .Include(o => o.Locations)
                    .Include(o => o.OrganizationTypes)
                    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                    .ToListAsync();

                //var query = await _context.Organizations
                //    //.Where(x => x.OrganizationId == id)
                //    .Where(o => o.ParentOrganizationId == id && userOrganizations.Contains(o.OrganizationId))
                //    .Include(o => o.Locations)
                //    .Include(o => o.OrganizationTypes)
                //    .Include(o => o.Organizations).ThenInclude(o => o.OrganizationTypes)
                //    .ToListAsync();

                return View(query);
            }   

        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .Include(o => o.Locations)
                .Include(o => o.OrganizationTypes)
                .Include(o => o.ParentOrganizations)
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // GET: Organizations/Create
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                id = await GetSelectedOrganization();

                if (id == null)
                {
                    return RedirectToAction("SelectOrganization", "Organizations");
                }
            }
            ViewData["ParentId"] = id;

            var organization = await _context.Organizations
                .Include(x => x.ParentOrganizations)
                .Where(x => x.OrganizationId == id)
                .FirstOrDefaultAsync();

            if (organization == null)
            {
                return NotFound();
            }

            var locationTypes = _context.LocationTypes.Where(x => x.LocationLevel != 1);
            ViewData["RefLocationTypes"] = locationTypes;
            ViewData["RefLocationTypesCount"] = locationTypes.Count();

            ViewData["RefLocationId"] = new SelectList(_context
                .Locations
                .Include(x => x.LocationTypes)
                .Where(x =>
                    x.LocationTypes!.LocationLevel == 2)
                .Select(x => new
                {
                    x.RefLocationId,
                    x.LocationName
                }), "RefLocationId", "LocationName");

            int? organizationTypeId = organization.RefOrganizationTypeId + 1;
            if (organizationTypeId == null)
            {
                return NotFound();
            }

            ViewData["OrganizationType"] = await _context.OrganizationTypes.Where(x => x.RefOrganizationTypeId == organizationTypeId).Select(x => x.OrganizationType).FirstOrDefaultAsync();

            ViewData["RefOrganizationTypeId"] = organizationTypeId;
            
            ViewData["ParentOrganizationId"] = organization.ParentOrganizationId;

            return View();
        }

        // POST: Organizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrganizationId,RegistrationDate,Code,OrganizationName,RefOrganizationTypeId,RefLocationId,Address,ParentOrganizationId,Latitude,Longitude,IsOrganizationUnit")] Organization organization, string[] RefLocationId)
        {
            if (ModelState.IsValid)
            {
                organization.OrganizationId = Guid.NewGuid();
                // Gets the last non-null item in RefLocationId array,
                // the array might have null values for locations left empty
                // and this assigns the last non-null location to RefLocationId
                if (RefLocationId.Length != 0)
                {
                    var location =
                    (from r in RefLocationId where !string.IsNullOrEmpty(r) select r)
                    .OrderByDescending(r => r).Count();

                    organization.RefLocationId = RefLocationId[location - 1];
                }

                _context.Add(organization);
                await _context.SaveChangesAsync();
                //TODO:
                if (organization.RefOrganizationTypeId == 5)
                {
                    return RedirectToAction(nameof(Index), new { controller = "Schools", id = organization.OrganizationId });
                }
                return RedirectToAction(nameof(Index), new { id = organization.OrganizationId });
            }
            ViewData["RefLocationId"] = new SelectList(_context.Locations, "RefLocationId", "RefLocationId", organization.RefLocationId);
            ViewData["RefOrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "RefOrganizationTypeId", "OrganizationType", organization.RefOrganizationTypeId);
            ViewData["ParentOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Code", organization.ParentOrganizationId);
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .Include(x => x.ParentOrganizations)
                .Where(x => x.OrganizationId == id)
                .FirstOrDefaultAsync();

            if (organization == null)
            {
                return NotFound();
            }

            var locationTypes = _context.LocationTypes.Where(x => x.LocationLevel != 1);
            ViewData["RefLocationTypes"] = locationTypes;
            ViewData["RefLocationTypesCount"] = locationTypes.Count();

            ViewData["RefLocationId"] = new SelectList(_context
                .Locations
                .Include(x => x.LocationTypes)
                .Where(x =>
                    x.LocationTypes!.LocationLevel == 2)
                .Select(x => new
                {
                    x.RefLocationId,
                    x.LocationName
                }), "RefLocationId", "LocationName", organization.RefLocationId);

            //Get Location Parents (only the lower level RefLocationId is saved, 
            //this gets the location parents for the saved location
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
            var allLocations = _context.Locations.Where(x => x.RefLocationTypeId != 1);

            //var locations = ListLocations(allLocations, organization.RefLocationId!);
            ViewData["RefLocationParents"] = organizationLocations;

            ViewData["ParentId"] = organization.ParentOrganizationId;
            ViewData["RefOrganizationTypeId"] = organization.RefOrganizationTypeId;
            ViewData["ParentOrganizationId"] = organization.ParentOrganizationId;

            return View(organization);
        }

        // POST: Organizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OrganizationId,RegistrationDate,Code,OrganizationName,RefOrganizationTypeId,RefLocationId,Address,ParentOrganizationId,Latitude,Longitude,IsOrganizationUnit")] Organization organization, string[] RefLocationId)
        {
            if (id != organization.OrganizationId)
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

                        organization.RefLocationId = RefLocationId[location - 1];
                    }
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.OrganizationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = organization.ParentOrganizationId });
            }
            ViewData["RefLocationId"] = new SelectList(_context.Locations, "RefLocationId", "RefLocationId", organization.RefLocationId);
            ViewData["RefOrganizationTypeId"] = new SelectList(_context.OrganizationTypes, "RefOrganizationTypeId", "OrganizationType", organization.RefOrganizationTypeId);
            ViewData["ParentOrganizationId"] = new SelectList(_context.Organizations, "OrganizationId", "Code", organization.ParentOrganizationId);
            return View(organization);
        }

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .Include(o => o.Locations)
                .Include(o => o.OrganizationTypes)
                .Include(o => o.ParentOrganizations)
                .FirstOrDefaultAsync(m => m.OrganizationId == id);
            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // POST: Organizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Organizations'  is null.");
            }
            var organization = await _context.Organizations.FindAsync(id);
            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(Guid id)
        {
          return (_context.Organizations?.Any(e => e.OrganizationId == id)).GetValueOrDefault();
        }
    }
}
