using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Gradebooks;
using ePTS.Entities.Core;
using ePTS.Models.ViewModels;
using ePTS.Entities.Assessments;
using ePTS.Entities.Reference;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
//using static ePTS.Web.Controllers.SchoolsController;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class GradebooksController : BaseController
    {
        //private readonly ApplicationDbContext _context;

        public GradebooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<GradebooksController> logger) : base(context, logger, userManager)
        {
            //_context = context;
        }

        // GET: Gradebooks
        public async Task<IActionResult> Index(Guid? id)
        {
            // id is the SchoolAcademicYearId
            if (id == null || _context.SchoolAcademicYears == null)
            {
                return NotFound();
            }

            ViewData["NextController"] = "GradebookAssessments";
            ViewData["ParentController"] = "SchoolAcademicYears";
            ViewData["ParentId"] = id;

            var selectedOrganizationId = await GetSelectedOrganization();

            if (selectedOrganizationId == null)
            {
                return RedirectToAction("SelectOrganization", "Organizations");
            }

            var userId = await GetCurrentUserIdAsync();
            var userOrganizationIds = await GetUserOrganizationIdsAsync(userId);
            var childOrganizationIds = await GetChildOrganizationIdsAsync(userOrganizationIds, selectedOrganizationId);

            var organization = await (from sa in _context.SchoolAcademicYears
                                      where sa.SchoolAcademicYearId == id
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
                CurrentLevel = 6,
                CurrentController = "Gradebooks",
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
            };

            var gradebooks = await _context.Gradebooks
                .Where(x => x.SchoolAcademicYearId == id)
                .Select(x => new GradebookViewModel
                {
                    GradeLevel = x.GradeLevels!.GradeLevel,
                    GradebookAssessmentsCount = x.GradebookAssessments.Count(),
                    GradebookId = x.GradebookId,
                    SchoolAcademicYearId = x.SchoolAcademicYearId,
                    IsMissingGradebookAssessments = x.IsMissingGradebookAssessments,
                    RegistrationDate = x.RegistrationDate,
                    Platform = x.AssessmentPlatformTypes!.AssessmentPlatformType,
                    SortOrder = x.GradeLevels!.SortOrder
                })
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            if (gradebooks == null)
            {
                return NotFound();
            }

            var model = new GradebookSummaryViewModel
            {
                InfoPanel = infoPanel,
                Gradebooks = gradebooks
            };

            return View(model);
        }

        // GET: Gradebooks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Gradebooks == null)
            {
                return NotFound();
            }

            var gradebook = await _context.Gradebooks
                .Include(g => g.AssessmentPlatformTypes)
                .Include(g => g.Assessments)
                .Include(g => g.GradeLevels)
                .Include(g => g.GradebookPeriods)
                .Include(g => g.GradebookStatus)
                .Include(g => g.SchoolAcademicYears)
                .FirstOrDefaultAsync(m => m.GradebookId == id);
            if (gradebook == null)
            {
                return NotFound();
            }

            return View(gradebook);
        }

        // GET: Gradebooks/Create
        public async Task<IActionResult> Create(Guid? id) // id is SchoolAcademicYearId
        {
            if (id == null)
            {
                return NotFound();
            }

            var existingGradebooks = await _context.Gradebooks
                .Where(x => x.SchoolAcademicYearId == id)
                .Select(x => x.RefGradeLevelId)
                .ToListAsync();

            //return view data for ViewData["RefGradeLevelId"] where RefGradeLevelId is not in existingGradebooks
            ViewData["RefGradeLevelId"] = new SelectList(_context.GradeLevels
                               .Where(x => !existingGradebooks.Contains(x.RefGradeLevelId)), "RefGradeLevelId", "GradeLevel");
            ViewData["ParentId"] = id;
            //TODO: set platform type to 1 for now

            return View();
        }

        // POST: Gradebooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradebookId,SchoolAcademicYearId,RefGradeLevelId,RegistrationDate,GradebookPeriodId,AssessmentId,RefAssessmentPlatformTypeId,RefGradebookStatusId,IsMissingGradebookAssessments")] Gradebook gradebook)
        {
            if (ModelState.IsValid)
            {

                var assessmentId = await _context.Assessments
                    .Where(x => x.RefGradeLevelId == gradebook.RefGradeLevelId)
                    .Select(x => x.AssessmentId)
                    .FirstOrDefaultAsync();

                if (assessmentId == Guid.Empty)
                {
                    return NotFound();
                }

                var gradebookPeriodId = await _context.GradebookPeriods
                    .Where(x => x.RefGradeLevelId == gradebook.RefGradeLevelId)
                    .Select(x => x.GradebookPeriodId)
                    .FirstOrDefaultAsync();

                if (gradebookPeriodId == Guid.Empty)
                {
                    return NotFound();
                }

                gradebook.GradebookId = Guid.NewGuid();
                gradebook.RefAssessmentPlatformTypeId = 1;
                gradebook.GradebookPeriodId = gradebookPeriodId;
                gradebook.RefGradebookStatusId = 1;
                gradebook.IsMissingGradebookAssessments = true;
                gradebook.AssessmentId = assessmentId;

                List<AssessmentItem> assessmentItems = await _context.AssessmentItems
                        .Where(x => x.AssessmentId == gradebook.AssessmentId)
                        .ToListAsync();

                List<GradebookAssessmentPeriod> gradebookAssessmentPeriods = await _context.GradebookAssessmentPeriods
                    .Where(x => x.GradebookPeriodId == gradebook.GradebookPeriodId)
                    .ToListAsync();

                //TODO
                List<RefPerformanceLevel> performanceLevels = await _context.PerformanceLevels
                    .ToListAsync();
                //TODO
                List<RefSex> sexes = await _context.Sex
                    .ToListAsync();

                List<RefParticipantType> participantTypes = await _context.ParticipantTypes
                    .ToListAsync();

                // foreach gradebookPeriod, create a gradebook assessment
                foreach (GradebookAssessmentPeriod gradebookAssessmentPeriod in gradebookAssessmentPeriods)
                {
                    GradebookAssessment gradebookAssessment = new()
                    {
                        GradebookAssessmentId = Guid.NewGuid(),
                        GradebookId = gradebook.GradebookId,
                        RegistrationDate = gradebook.RegistrationDate,
                        // Set as missing assessment results by default
                        IsMissingAssessmentResults = true,
                        // Set status as pending by default, will be updated when assessment results are uploaded
                        RefGradebookAssessmentStatusId = 2,
                        GradebookAssessmentPeriodId = gradebookAssessmentPeriod.GradebookAssessmentPeriodId
                    };

                    _context.Add(gradebookAssessment);

                    // foreach assessmentItem within each gradebookassessment, create an assessment result for each assessment item.
                    foreach (AssessmentItem assessmentItem in assessmentItems)
                    {
                        AssessmentResult assessmentResult = new()
                        {
                            AssessmentResultId = Guid.NewGuid(),
                            AssessmentItemId = assessmentItem.AssessmentItemId,
                            RegistrationDate = gradebook.RegistrationDate,
                            GradebookAssessmentId = gradebookAssessment.GradebookAssessmentId,
                        };

                        _context.Add(assessmentResult);
                    }

                    // foreach gradebook assessment, performance level and sex, create an assessment performance level.
                    foreach (RefPerformanceLevel performanceLevel in performanceLevels)
                    {
                        foreach (RefSex sex in sexes)
                        {
                            AssessmentPerformanceLevel assessmentPerformanceLevel = new()
                            {
                                AssessmentPerformanceLevelId = Guid.NewGuid(),
                                RefPerformanceLevelId = performanceLevel.RefPerformanceLevelId,
                                RefSexId = sex.RefSexId,
                                RegistrationDate = gradebook.RegistrationDate,
                                GradebookAssessmentId = gradebookAssessment.GradebookAssessmentId,
                            };

                            _context.Add(assessmentPerformanceLevel);
                        }
                    }
                }

                foreach (RefParticipantType participantType in participantTypes)
                {
                    GradebookEnrollment gradebookEnrollment = new()
                    {
                        RegistrationDate = gradebook.RegistrationDate,
                        GradebookEnrollmentId = Guid.NewGuid(),
                        GradebookId = gradebook.GradebookId,
                        RefParticipantTypeId = participantType.RefParticipantTypeId,
                    };
                    _context.Add(gradebookEnrollment);
                }

                _context.Add(gradebook);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), new { id = gradebook.SchoolAcademicYearId });
            }
            ViewData["RefAssessmentPlatformTypeId"] = new SelectList(_context.AssessmentPlatformTypes, "RefAssessmentPlatformTypeId", "AssessmentPlatformType", gradebook.RefAssessmentPlatformTypeId);
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "AssessmentId", "AssessmentId", gradebook.AssessmentId);
            ViewData["RefGradeLevelId"] = new SelectList(_context.GradeLevels, "RefGradeLevelId", "GradeLevel", gradebook.RefGradeLevelId);
            ViewData["GradebookPeriodId"] = new SelectList(_context.GradebookPeriods, "GradebookPeriodId", "GradebookPeriodName", gradebook.GradebookPeriodId);
            ViewData["RefGradebookStatusId"] = new SelectList(_context.GradebookStatus, "RefGradebookStatusId", "GradebookStatus", gradebook.RefGradebookStatusId);
            return View(gradebook);
        }

        // GET: Gradebooks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Gradebooks == null)
            {
                return NotFound();
            }

            var gradebook = await _context.Gradebooks.FindAsync(id);
            if (gradebook == null)
            {
                return NotFound();
            }
            ViewData["RefAssessmentPlatformTypeId"] = new SelectList(_context.AssessmentPlatformTypes, "RefAssessmentPlatformTypeId", "AssessmentPlatformType", gradebook.RefAssessmentPlatformTypeId);
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "AssessmentId", "AssessmentId", gradebook.AssessmentId);
            ViewData["RefGradeLevelId"] = new SelectList(_context.GradeLevels, "RefGradeLevelId", "GradeLevel", gradebook.RefGradeLevelId);
            ViewData["GradebookPeriodId"] = new SelectList(_context.GradebookPeriods, "GradebookPeriodId", "GradebookPeriodName", gradebook.GradebookPeriodId);
            ViewData["RefGradebookStatusId"] = new SelectList(_context.GradebookStatus, "RefGradebookStatusId", "GradebookStatus", gradebook.RefGradebookStatusId);
            ViewData["SchoolAcademicYearId"] = new SelectList(_context.SchoolAcademicYears, "SchoolAcademicYearId", "SchoolAcademicYearId", gradebook.SchoolAcademicYearId);
            return View(gradebook);
        }

        // POST: Gradebooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GradebookId,SchoolAcademicYearId,RefGradeLevelId,RegistrationDate,GradebookPeriodId,AssessmentId,RefAssessmentPlatformTypeId,RefGradebookStatusId,IsMissingGradebookAssessments")] Gradebook gradebook)
        {
            if (id != gradebook.GradebookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradebookExists(gradebook.GradebookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index), new { gradebook.SchoolAcademicYearId });
            }
            ViewData["RefAssessmentPlatformTypeId"] = new SelectList(_context.AssessmentPlatformTypes, "RefAssessmentPlatformTypeId", "AssessmentPlatformType", gradebook.RefAssessmentPlatformTypeId);
            ViewData["AssessmentId"] = new SelectList(_context.Assessments, "AssessmentId", "AssessmentId", gradebook.AssessmentId);
            ViewData["RefGradeLevelId"] = new SelectList(_context.GradeLevels, "RefGradeLevelId", "GradeLevel", gradebook.RefGradeLevelId);
            ViewData["GradebookPeriodId"] = new SelectList(_context.GradebookPeriods, "GradebookPeriodId", "GradebookPeriodName", gradebook.GradebookPeriodId);
            ViewData["RefGradebookStatusId"] = new SelectList(_context.GradebookStatus, "RefGradebookStatusId", "GradebookStatus", gradebook.RefGradebookStatusId);
            ViewData["SchoolAcademicYearId"] = new SelectList(_context.SchoolAcademicYears, "SchoolAcademicYearId", "SchoolAcademicYearId", gradebook.SchoolAcademicYearId);
            return View(gradebook);
        }

        // GET: Gradebooks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Gradebooks == null)
            {
                return NotFound();
            }

            var gradebook = await _context.Gradebooks
                .Include(g => g.AssessmentPlatformTypes)
                .Include(g => g.Assessments)
                .Include(g => g.GradeLevels)
                .Include(g => g.GradebookPeriods)
                .Include(g => g.GradebookStatus)
                .Include(g => g.SchoolAcademicYears)
                .FirstOrDefaultAsync(m => m.GradebookId == id);
            ViewData["ParentId"] = gradebook.SchoolAcademicYearId;
            if (gradebook == null)
            {
                return NotFound();
            }

            return View(gradebook);
        }

        // POST: Gradebooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Gradebooks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Gradebooks'  is null.");
            }
            var gradebook = await _context.Gradebooks.FindAsync(id);
            if (gradebook != null)
            {
                _context.Gradebooks.Remove(gradebook);
            }

            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index), new { id = gradebook.SchoolAcademicYearId });
        }

        private bool GradebookExists(Guid id)
        {
            return (_context.Gradebooks?.Any(e => e.GradebookId == id)).GetValueOrDefault();
        }
    }
}
