using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Assessments;
using ePTS.Entities.Gradebooks;
using ePTS.Models.ViewModels;
using ePTS.Entities.Core;
using ePTS.Web.Extensions;
using System.Collections;
using ePTS.Web.Models;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class AssessmentResultsController : BaseController
    {
        public AssessmentResultsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AssessmentResultsController> logger) : base(context, logger, userManager)
        {
        }


        // GET: AssessmentResults
        public async Task<IActionResult> Index(Guid? id)
        {
            //id is GradebookAssessmentId
            if (id == null)
            {
                return NotFound();
            }

            ViewData["NextController"] = "AssessmentResults";
            ViewData["ParentController"] = "GradebookAssessments";

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);

            var organization = await
                (from ar in _context.AssessmentResults
                 where ar.GradebookAssessmentId == id
                 join ga in _context.GradebookAssessments on ar.GradebookAssessmentId equals ga.GradebookAssessmentId
                 join g in _context.Gradebooks on ga.GradebookId equals g.GradebookId
                 join sa in _context.SchoolAcademicYears on g.SchoolAcademicYearId equals sa.SchoolAcademicYearId
                 join s in _context.Schools on sa.OrganizationId equals s.OrganizationId into schoolGroup
                 from sg in schoolGroup.DefaultIfEmpty()
                 select new InfoPanelViewModel
                 {
                     OrganizationId = sg.OrganizationId,
                     OrganizationName = sg.OrganizationName,
                     OrganizationType = sg.OrganizationTypes == null ? null : sg.OrganizationTypes.OrganizationType,
                     Code = sg.Code,
                     IsOrganizationUnit = sg.IsOrganizationUnit,
                     RefLocationId = sg.RefLocationId,
                     Zone = sg.ParentOrganizations == null ? null : sg.ParentOrganizations.OrganizationName,
                     Address = sg.Address,
                     Latitude = sg.Latitude,
                     Longitude = sg.Longitude,
                     //EMIS = sg.SchoolCode,
                     SchoolType = sg.SchoolTypes == null ? null : sg.SchoolTypes.SchoolType,
                     SchoolLocation = sg.SchoolLocations == null ? null : sg.SchoolLocations.SchoolLocation,
                     SchoolAdministrationType = sg.SchoolAdministrationTypes == null ? null : sg.SchoolAdministrationTypes.SchoolAdministrationType,
                     SchoolLanguage = sg.SchoolLanguages == null ? null : sg.SchoolLanguages.SchoolLanguage,
                     AcademicYear = sa.AcademicYears == null ? null : sa.AcademicYears.AcademicYear,
                     SchoolAcademicYearId = sa.SchoolAcademicYearId,
                     GradeLevel = g.GradeLevels == null ? null : g.GradeLevels.GradeLevel,
                     AssessmentTerm = ga.GradebookAssessmentPeriods == null ? null : ga.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                     AssessmentWeek = ga.GradebookAssessmentPeriods == null ? null : ga.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek,
                     GradebookId = ga.GradebookId,
                     GradebookAssessmentId = ga.GradebookAssessmentId,
                     AssessedFemale = ga.AssessedFemale,
                     AssessedMale = ga.AssessedMale,
                     Assessed = ga.AssessedMale + ga.AssessedFemale

                 })
                .FirstOrDefaultAsync();

            if (organization == null)
            {
                return NotFound();
            }

            var organizationHierarchy = organization.OrganizationId.HasValue
                ? await GetBreadcrumbAsync((Guid)organization.OrganizationId)
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
                organizationParents = (IEnumerable<OrganizationParentViewModel>)await GetParentHierarchyAsync((Guid)organization.OrganizationId!);
            }
            else
            {
                organizationParents = null;
            }

            var infoPanel = new InfoPanelViewModel
            {
                CurrentLevel = 8,
                CurrentController = "AssessmentResults",
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationType = organization.OrganizationType,
                Code = organization.Code,
                IsOrganizationUnit = organization.IsOrganizationUnit,
                OrganizationParent = organizationHierarchy, //organizationParents,
                LocationParent = organizationLocations,
                UserOrganizationId = userOrganizationIds.Contains((Guid)organization.OrganizationId) ? organization.OrganizationId : null,
                Zone = organization.Zone,
                Address = organization.Address,
                Latitude = organization.Latitude,
                Longitude = organization.Longitude,
                //EMIS = organization.SchoolCode,
                SchoolType = organization.SchoolType,
                SchoolLocation = organization.SchoolLocation,
                SchoolAdministrationType = organization.SchoolAdministrationType,
                SchoolLanguage = organization.SchoolLanguage,
                AcademicYear = organization.AcademicYear,
                SchoolAcademicYearId = organization.SchoolAcademicYearId,
                GradeLevel = organization.GradeLevel,
                AssessmentTerm = organization.AssessmentTerm,
                AssessmentWeek = organization.AssessmentWeek,
                GradebookId = organization.GradebookId,
                GradebookAssessmentId = organization.GradebookAssessmentId,
                AssessedFemale = organization.AssessedFemale,
                AssessedMale = organization.AssessedMale,
                Assessed = organization.Assessed
            };


            var assessmentResults = _context.AssessmentResults
                .Where(x => x.GradebookAssessmentId == id)
                .Select(x => new AssessmentResultsViewModel
                {
                    GradeLevel = x.GradebookAssessments!.Gradebooks!.GradeLevels!.GradeLevel,
                    AssessmentTerm = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                    AssessmentWeek = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek,
                    AssessmentItemText = x.AssessmentItems!.AssessmentItemText,
                    AssessmentCategory = x.AssessmentItems!.AssessmentCategories!.AssessmentCategory,
                    Score = x.Score,
                    SortOrder = x.AssessmentItems.SortOrder,
                    AssessmentResultId = x.AssessmentResultId,
                })
                .OrderBy(x => x.SortOrder);

            if (assessmentResults == null)
            {
                return NotFound();
            }

            var assessmentPerformanceLevels = _context.AssessmentPerformanceLevels
                .Where(x => x.GradebookAssessmentId == id)
                .Select(x => new AssessmentPerformanceLevelsViewModel
                {
                    PerformanceLevel = x.PerformanceLevels!.PerformanceLevel,
                    Sex = x.Sex!.Sex,
                    GradebookAssessmentId = x.GradebookAssessmentId,
                    Score = x.Score,
                    RefPerformanceLevelId = x.RefPerformanceLevelId,
                    RefSexId = x.RefSexId,
                    RegistrationDate = x.RegistrationDate,
                    Assessed = x.RefSexId == 1 ? x.GradebookAssessments!.AssessedMale :
                        x.RefSexId == 2 ? x.GradebookAssessments!.AssessedFemale : null,
                    Code = x.PerformanceLevels.Code,
                    Color = x.PerformanceLevels.Color,
                    SexId = x.Sex.SexId,
                    AssessmentPerformanceLevelId = x.AssessmentPerformanceLevelId,
                    PerformanceLevelSortOrder = x.PerformanceLevels.SortOrder,
                    SexSortOrder = x.Sex.SortOrder,
                    GradeLevel = x.GradebookAssessments!.Gradebooks!.GradeLevels!.GradeLevel,
                    AssessmentTerm = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                    AssessmentWeek = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek,
                    Percent = (x.RefSexId == 1 && x.GradebookAssessments!.AssessedMale != 0)
                                        ? Math.Round(((double)x.Score! / x.GradebookAssessments!.AssessedMale) * 100)
                                        : (x.RefSexId == 2 && x.GradebookAssessments!.AssessedFemale != 0)
                                            ? Math.Round(((double)x.Score! / x.GradebookAssessments!.AssessedFemale) * 100)
                                            : null
                })
                .OrderBy(a => a.PerformanceLevelSortOrder).ThenBy(x => x.SexSortOrder);

            if (assessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var model = new AssessmentSummaryViewModel
            {
                InfoPanel = infoPanel,
                AssessmentResults = await assessmentResults.OrderBy(x => x.SortOrder).ToListAsync(),
                AssessmentPerformanceLevels = await assessmentPerformanceLevels.ToListAsync()
            };

            ViewData["ParentId"] = infoPanel.GradebookId;

            return View(model);
        }

        // GET: AssessmentResults/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AssessmentResults == null)
            {
                return NotFound();
            }

            var assessmentResult = await _context.AssessmentResults
                .Include(a => a.AssessmentItems)
                .Include(a => a.GradebookAssessments)
                .FirstOrDefaultAsync(m => m.AssessmentResultId == id);
            if (assessmentResult == null)
            {
                return NotFound();
            }

            return View(assessmentResult);
        }

        // GET: AssessmentResults/Create
        public IActionResult Create()
        {
            ViewData["AssessmentItemId"] = new SelectList(_context.AssessmentItems, "AssessmentItemId", "AssessmentItemId");
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId");
            return View();
        }

        // POST: AssessmentResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssessmentResultId,GradebookAssessmentId,RegistrationDate,AssessmentItemId,ScoreFemale,ScoreMale,Score")] AssessmentResult assessmentResult)
        {
            if (ModelState.IsValid)
            {
                assessmentResult.AssessmentResultId = Guid.NewGuid();
                _context.Add(assessmentResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssessmentItemId"] = new SelectList(_context.AssessmentItems, "AssessmentItemId", "AssessmentItemId", assessmentResult.AssessmentItemId);
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId", assessmentResult.GradebookAssessmentId);
            return View(assessmentResult);
        }

        // GET: AssessmentResults/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AssessmentResults == null)
            {
                return NotFound();
            }

            ViewData["ParentId"] = id;

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);

            var organization = await
                (from ar in _context.AssessmentResults
                 where ar.GradebookAssessmentId == id
                 join ga in _context.GradebookAssessments on ar.GradebookAssessmentId equals ga.GradebookAssessmentId
                 join g in _context.Gradebooks on ga.GradebookId equals g.GradebookId
                 join sa in _context.SchoolAcademicYears on g.SchoolAcademicYearId equals sa.SchoolAcademicYearId
                 join s in _context.Schools on sa.OrganizationId equals s.OrganizationId into schoolGroup
                 from sg in schoolGroup.DefaultIfEmpty()
                 select new InfoPanelViewModel
                 {
                     OrganizationId = sg.OrganizationId,
                     OrganizationName = sg.OrganizationName,
                     OrganizationType = sg.OrganizationTypes == null ? null : sg.OrganizationTypes.OrganizationType,
                     Code = sg.Code,
                     IsOrganizationUnit = sg.IsOrganizationUnit,
                     RefLocationId = sg.RefLocationId,
                     Zone = sg.ParentOrganizations == null ? null : sg.ParentOrganizations.OrganizationName,
                     Address = sg.Address,
                     Latitude = sg.Latitude,
                     Longitude = sg.Longitude,
                     //EMIS = sg.SchoolCode,
                     SchoolType = sg.SchoolTypes == null ? null : sg.SchoolTypes.SchoolType,
                     SchoolLocation = sg.SchoolLocations == null ? null : sg.SchoolLocations.SchoolLocation,
                     SchoolAdministrationType = sg.SchoolAdministrationTypes == null ? null : sg.SchoolAdministrationTypes.SchoolAdministrationType,
                     SchoolLanguage = sg.SchoolLanguages == null ? null : sg.SchoolLanguages.SchoolLanguage,
                     AcademicYear = sa.AcademicYears == null ? null : sa.AcademicYears.AcademicYear,
                     GradeLevel = g.GradeLevels == null ? null : g.GradeLevels.GradeLevel,
                     AssessmentTerm = ga.GradebookAssessmentPeriods == null ? null : ga.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                     AssessmentWeek = ga.GradebookAssessmentPeriods == null ? null : ga.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek,
                     GradebookId = ga.GradebookId,
                     GradebookAssessmentId = ga.GradebookAssessmentId,
                     AssessedFemale = ga.AssessedFemale,
                     AssessedMale = ga.AssessedMale,
                     Assessed = ga.AssessedMale + ga.AssessedFemale


                 })
                .FirstOrDefaultAsync();

            if (organization == null)
            {
                return NotFound();
            }

            var organizationHierarchy = organization.OrganizationId.HasValue
                ? await GetBreadcrumbAsync((Guid)organization.OrganizationId)
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
                organizationParents = (IEnumerable<OrganizationParentViewModel>)await GetParentHierarchyAsync((Guid)organization.OrganizationId!);
            }
            else
            {
                organizationParents = null;
            }

            var infoPanel = new InfoPanelViewModel
            {
                CurrentLevel = 8,
                CurrentController = "AssessmentResults",
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.OrganizationName,
                OrganizationType = organization.OrganizationType,
                Code = organization.Code,
                IsOrganizationUnit = organization.IsOrganizationUnit,
                OrganizationParent = organizationHierarchy, //organizationParents,
                LocationParent = organizationLocations,
                UserOrganizationId = userOrganizationIds.Contains((Guid)organization.OrganizationId) ? organization.OrganizationId : null,
                Zone = organization.Zone,
                Address = organization.Address,
                Latitude = organization.Latitude,
                Longitude = organization.Longitude,
                //EMIS = organization.SchoolCode,
                SchoolType = organization.SchoolType,
                SchoolLocation = organization.SchoolLocation,
                SchoolAdministrationType = organization.SchoolAdministrationType,
                SchoolLanguage = organization.SchoolLanguage,
                AcademicYear = organization.AcademicYear,
                GradeLevel = organization.GradeLevel,
                AssessmentTerm = organization.AssessmentTerm,
                AssessmentWeek = organization.AssessmentWeek,
                GradebookId = organization.GradebookId,
                GradebookAssessmentId = organization.GradebookAssessmentId,
                AssessedFemale = organization.AssessedFemale,
                AssessedMale = organization.AssessedMale,
                Assessed = organization.Assessed

            };

            var assessmentResults = _context.AssessmentResults
                .Where(x => x.GradebookAssessmentId == id)
                //.OrderBy(a => a.AssessmentItems.SortOrder)
                .Select(x => new AssessmentResultsEditModel
                {
                    AssessmentTerm = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                    AssessmentWeek = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek,
                    AssessmentItemText = x.AssessmentItems!.AssessmentItemText,
                    AssessmentCategory = x.AssessmentItems!.AssessmentCategories!.AssessmentCategory,
                    Score = x.Score,
                    SortOrder = x.AssessmentItems.SortOrder,
                    AssessmentResultId = x.AssessmentResultId,
                    GradebookAssessmentId = x.GradebookAssessmentId,
                    RegistrationDate = x.RegistrationDate,
                    AssessmentItemId = x.AssessmentItemId,
                })
                .OrderBy(x => x.SortOrder);

            if (assessmentResults == null)
            {
                return NotFound();
            }


            var assessmentPerformanceLevels = _context.AssessmentPerformanceLevels
                .Where(x => x.GradebookAssessmentId == id)
                .Select(x => new AssessmentPerformanceLevelsEditModel
                {
                    PerformanceLevel = x.PerformanceLevels!.PerformanceLevel,
                    PerformanceLevelText = x.PerformanceLevels!.PerformanceLevelText,
                    Sex = x.Sex!.Sex,
                    GradebookAssessmentId = x.GradebookAssessmentId,
                    Score = x.Score,
                    RefPerformanceLevelId = x.RefPerformanceLevelId,
                    RefSexId = x.RefSexId,
                    RegistrationDate = x.RegistrationDate,
                    Assessed = x.RefSexId == 1 ? x.GradebookAssessments!.AssessedMale :
                        x.RefSexId == 2 ? x.GradebookAssessments!.AssessedFemale : null,
                    Code = x.PerformanceLevels.Code,
                    Color = x.PerformanceLevels.Color,
                    SexId = x.Sex.SexId,
                    AssessmentPerformanceLevelId = x.AssessmentPerformanceLevelId,
                    PerformanceLevelSortOrder = x.PerformanceLevels.SortOrder,
                    SexSortOrder = x.Sex.SortOrder,
                    Term = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentTerms!.AssessmentTerm,
                    Week = x.GradebookAssessments!.GradebookAssessmentPeriods!.AssessmentWeeks!.AssessmentWeek
                })
                .OrderBy(a => a.PerformanceLevelSortOrder).ThenBy(x => x.SexSortOrder);

            if (assessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var model = new AssessmentSummaryEditModel
            {
                InfoPanel = infoPanel,
                AssessmentResults = await assessmentResults.OrderBy(x => x.SortOrder).ToListAsync(),
                AssessmentPerformanceLevels = await assessmentPerformanceLevels.ToListAsync()
            };

            return View(model);
        }

        // POST: AssessmentResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, 
            DateTime registrationDate,
            List<AssessmentResultsEditModel> results, 
            List<AssessmentPerformanceLevelsEditModel> performance
            )
        {

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (AssessmentResultsEditModel item in results)
                    {
                        AssessmentResult? Exists_AssessmentResult = await _context.AssessmentResults.FindAsync(item.AssessmentResultId);

                        if (Exists_AssessmentResult != null)
                        {
                            Exists_AssessmentResult.AssessmentItemId = item.AssessmentItemId;
                            Exists_AssessmentResult.Score = item.Score;
                            Exists_AssessmentResult.RegistrationDate = registrationDate;
                        }
                    }

                    foreach (AssessmentPerformanceLevelsEditModel item in performance)
                    {
                        AssessmentPerformanceLevel? Exists_AssessmentPerformanceLevel = await _context.AssessmentPerformanceLevels.FindAsync(item.AssessmentPerformanceLevelId);

                        if (Exists_AssessmentPerformanceLevel != null)
                        {
                            Exists_AssessmentPerformanceLevel.RefPerformanceLevelId = item.RefPerformanceLevelId;
                            Exists_AssessmentPerformanceLevel.RefSexId = item.RefSexId;
                            Exists_AssessmentPerformanceLevel.Score = item.Score;
                            Exists_AssessmentPerformanceLevel.RegistrationDate = registrationDate;
                        }
                    }

                    //_context.Update(assessment);
                    await _context.SaveChangesAsync();

                    TempData["messageType"] = "success";
                    TempData["messageTitle"] = "RECORDS UPDATED";
                    TempData["message"] = "Records updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!AssessmentResultExists(assessmentResult.AssessmentResultId))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), new { id });
            }
            
            return View();
        }

        // GET: AssessmentResults/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AssessmentResults == null)
            {
                return NotFound();
            }

            var assessmentResult = await _context.AssessmentResults
                .Include(a => a.AssessmentItems)
                .Include(a => a.GradebookAssessments)
                .FirstOrDefaultAsync(m => m.AssessmentResultId == id);
            if (assessmentResult == null)
            {
                return NotFound();
            }

            return View(assessmentResult);
        }

        // POST: AssessmentResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AssessmentResults == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AssessmentResults'  is null.");
            }
            var assessmentResult = await _context.AssessmentResults.FindAsync(id);
            if (assessmentResult != null)
            {
                _context.AssessmentResults.Remove(assessmentResult);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentResultExists(Guid id)
        {
            return (_context.AssessmentResults?.Any(e => e.AssessmentResultId == id)).GetValueOrDefault();
        }

        #region Helpers

        private async Task<IEnumerable> GetLocationHierarchyAsync(string refLocationId)
        {
            var allLocations = await _context.Locations
                .Include(x => x.LocationTypes)
                .Include(x => x.ParentLocations)
                .Select(x => new LocationParentViewModel
                {
                    Index = 1,
                    RefLocationId = x.RefLocationId,
                    LocationName = x.LocationName,
                    ParentLocationId = x.ParentLocationId,
                    ParentName = x.ParentLocations!.LocationName,
                    RefLocationTypeId = x.RefLocationTypeId,
                    LocationType = x.LocationTypes!.LocationType,
                    LocationLevel = x.LocationTypes.LocationLevel
                })
                .ToListAsync();

            var locations = ListLocations(allLocations, refLocationId);

            IEnumerable<LocationParentViewModel> sortedLocations = new List<LocationParentViewModel>();

            //Add index and Sort descending to "parents"
            sortedLocations = locations
                .Select((x, index) => new LocationParentViewModel
                {
                    Index = index,
                    RefLocationId = x.RefLocationId,
                    LocationName = x.LocationName,
                    ParentLocationId = x.ParentLocationId,
                    ParentName = x.ParentName,
                    RefLocationTypeId = x.RefLocationTypeId,
                    LocationType = x.LocationType,
                    LocationLevel = x.LocationLevel
                })
                .OrderByDescending(x => x.Index)
                .ToList();

            return sortedLocations.ToList();

        }

        public static IEnumerable<LocationParentViewModel> ListLocations(IEnumerable<LocationParentViewModel> list, string? id)
        {
            var current = list.Where(n => n.RefLocationId == id).FirstOrDefault();

            if (current == null) return Enumerable.Empty<LocationParentViewModel>();

            return Enumerable.Concat(new[] { current }, ListLocations(list, current.ParentLocationId));
        }

        #endregion
    }
}
