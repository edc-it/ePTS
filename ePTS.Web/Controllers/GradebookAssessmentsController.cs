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
//using static ePTS.Web.Controllers.GradebooksController;
using ePTS.Models.ViewModels;
using ePTS.Web.Extensions;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class GradebookAssessmentsController : BaseController
    {

        public GradebookAssessmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AssessmentResultsController> logger) : base(context, logger, userManager)
        {
        }

        // GET: GradebookAssessments
        public async Task<IActionResult> Index(Guid? id)
        {
            // id is GradebookId
            if (id == null)
            {
                return NotFound();
            }

            ViewData["NextController"] = "AssessmentResults";
            ViewData["ParentController"] = "Gradebooks";
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
                (from ga in _context.GradebookAssessments
                 where ga.GradebookId == id
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
                     SchoolAcademicYearId = sa.SchoolAcademicYearId,
                     GradebookId = g.GradebookId,
                     GradebookAssessmentId = ga.GradebookAssessmentId,
                     //IsMissingAssessmentResults = ga.IsMissingAssessmentResults

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
                CurrentLevel = 7,
                CurrentController = "GradebookAssessments",
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
                SchoolAcademicYearId = organization.SchoolAcademicYearId,
                GradebookId = organization.GradebookId,
                GradebookAssessmentId = organization.GradebookAssessmentId,
                //IsMissingAssessmentResults = organization.IsMissingAssessmentResults
            };

            var gradebookAssessments = await _context.GradebookAssessments
                .Where(x => x.GradebookId == id)
                .Select(x => new GradebookAssessmentViewModel
                {
                    GradeLevel = x.Gradebooks == null ? null : x.Gradebooks.GradeLevels!.GradeLevel,
                    AssessmentTerm = x.GradebookAssessmentPeriods == null ? null : x.GradebookAssessmentPeriods.AssessmentTerms!.AssessmentTerm,
                    AssessmentWeek = x.GradebookAssessmentPeriods == null ? null : x.GradebookAssessmentPeriods.AssessmentWeeks!.AssessmentWeek,
                    AssessedFemale = x.AssessedFemale,
                    AssessedMale = x.AssessedMale,
                    Assessed = x.AssessedFemale + x.AssessedMale,
                    GradebookAssessmentId = x.GradebookAssessmentId,
                    GradebookId = x.GradebookId,
                    GradebookAssessmentPeriodId = x.GradebookAssessmentPeriodId,
                    GradebookAssessmentStatusId = x.RefGradebookAssessmentStatusId,
                    GradebookAssessmentStatus = x.GradebookAssessmentStatus == null ? null : x.GradebookAssessmentStatus.GradebookAssessmentStatus,
                    SortOrder = x.GradebookAssessmentPeriods == null ? null : x.GradebookAssessmentPeriods.SortOrder,
                    IsMissingAssessmentResults = x.IsMissingAssessmentResults
                })
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            if (gradebookAssessments == null) return NotFound();

            var model = new GradebookAssessmentSummaryViewModel
            {
                InfoPanel = infoPanel,
                GradebookAssessments = gradebookAssessments
            };

            return View(model);
        }

        // GET: GradebookAssessments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.GradebookAssessments == null)
            {
                return NotFound();
            }

            var gradebookAssessment = await _context.GradebookAssessments
                .Include(g => g.GradebookAssessmentPeriods)
                .Include(g => g.GradebookAssessmentStatus)
                .Include(g => g.Gradebooks)
                .FirstOrDefaultAsync(m => m.GradebookAssessmentId == id);
            if (gradebookAssessment == null)
            {
                return NotFound();
            }

            return View(gradebookAssessment);
        }

        // GET: GradebookAssessments/Create
        public IActionResult Create()
        {
            ViewData["GradebookAssessmentPeriodId"] = new SelectList(_context.GradebookAssessmentPeriods, "GradebookAssessmentPeriodId", "GradebookAssessmentPeriod");
            ViewData["RefGradebookAssessmentStatusId"] = new SelectList(_context.GradebookAssessmentStatus, "RefGradebookAssessmentStatusId", "GradebookAssessmentStatus");
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId");
            return View();
        }

        // POST: GradebookAssessments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradebookAssessmentId,GradebookId,GradebookAssessmentPeriodId,RegistrationDate,AssessedFemale,AssessedMale,Assessed,AbsentFemale,AbsentMale,Absent,RefGradebookAssessmentStatusId,IsMissingAssessmentResults")] GradebookAssessment gradebookAssessment)
        {
            if (ModelState.IsValid)
            {
                gradebookAssessment.GradebookAssessmentId = Guid.NewGuid();
                _context.Add(gradebookAssessment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradebookAssessmentPeriodId"] = new SelectList(_context.GradebookAssessmentPeriods, "GradebookAssessmentPeriodId", "GradebookAssessmentPeriodId", gradebookAssessment.GradebookAssessmentPeriodId);
            ViewData["RefGradebookAssessmentStatusId"] = new SelectList(_context.GradebookAssessmentStatus, "RefGradebookAssessmentStatusId", "GradebookAssessmentStatus", gradebookAssessment.RefGradebookAssessmentStatusId);
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId", gradebookAssessment.GradebookId);
            return View(gradebookAssessment);
        }

        // GET: GradebookAssessments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.GradebookAssessments == null)
            {
                return NotFound();
            }

            var gradebookAssessment = await _context.GradebookAssessments.FindAsync(id);
            if (gradebookAssessment == null)
            {
                return NotFound();
            }
            ViewData["GradebookAssessmentPeriodId"] = gradebookAssessment.GradebookAssessmentPeriodId;
            ViewData["RefGradebookAssessmentStatusId"] = gradebookAssessment.RefGradebookAssessmentStatusId;
            ViewData["GradebookId"] = gradebookAssessment.GradebookId;
            ViewData["ParentId"] = gradebookAssessment.GradebookId;

            return View(gradebookAssessment);
        }

        // POST: GradebookAssessments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GradebookAssessmentId,GradebookId,GradebookAssessmentPeriodId,RegistrationDate,AssessedFemale,AssessedMale,Assessed,AbsentFemale,AbsentMale,Absent,RefGradebookAssessmentStatusId,IsMissingAssessmentResults")] GradebookAssessment gradebookAssessment)
        {
            if (id != gradebookAssessment.GradebookAssessmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    gradebookAssessment.IsMissingAssessmentResults = false;
                    _context.Update(gradebookAssessment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradebookAssessmentExists(gradebookAssessment.GradebookAssessmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = gradebookAssessment.GradebookId });
            }
            ViewData["GradebookAssessmentPeriodId"] = new SelectList(_context.GradebookAssessmentPeriods, "GradebookAssessmentPeriodId", "GradebookAssessmentPeriodId", gradebookAssessment.GradebookAssessmentPeriodId);
            ViewData["RefGradebookAssessmentStatusId"] = new SelectList(_context.GradebookAssessmentStatus, "RefGradebookAssessmentStatusId", "GradebookAssessmentStatus", gradebookAssessment.RefGradebookAssessmentStatusId);
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId", gradebookAssessment.GradebookId);
            return View(gradebookAssessment);
        }

        // GET: GradebookAssessments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.GradebookAssessments == null)
            {
                return NotFound();
            }

            var gradebookAssessment = await _context.GradebookAssessments
                .Include(g => g.GradebookAssessmentPeriods)
                .Include(g => g.GradebookAssessmentStatus)
                .Include(g => g.Gradebooks)
                .FirstOrDefaultAsync(m => m.GradebookAssessmentId == id);
            if (gradebookAssessment == null)
            {
                return NotFound();
            }

            return View(gradebookAssessment);
        }

        // POST: GradebookAssessments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.GradebookAssessments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GradebookAssessments'  is null.");
            }
            var gradebookAssessment = await _context.GradebookAssessments.FindAsync(id);
            if (gradebookAssessment != null)
            {
                _context.GradebookAssessments.Remove(gradebookAssessment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradebookAssessmentExists(Guid id)
        {
          return (_context.GradebookAssessments?.Any(e => e.GradebookAssessmentId == id)).GetValueOrDefault();
        }
    }
}
