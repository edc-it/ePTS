using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Gradebooks;
using Microsoft.AspNetCore.Authorization;
using ePTS.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using ePTS.Models.ViewModels;
using ePTS.Entities.Core;

namespace ePTS.Web.Controllers
{
    [Authorize]
    public class GradebookEnrollmentsController : BaseController
    {
        //private readonly ApplicationDbContext _context;

        public GradebookEnrollmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<GradebooksController> logger) : base(context, logger, userManager)
        {
            //_context = context;
        }

        // GET: GradebookEnrollments
        public async Task<IActionResult> Index(Guid? id)
        {
            // id is the GradebookId
            if (id == null)
            {
                return NotFound();
            }

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
                (from ge in _context.GradebookEnrollments
                 where ge.GradebookId == id
                 join g in _context.Gradebooks on ge.GradebookId equals g.GradebookId
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
                CurrentController = "GradebookEnrollments",
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

            };

            var gradebookEnrollments = await _context.GradebookEnrollments
                .Where(x => x.GradebookId == id)
                .Select(x => new GradebookEnrollmentViewModel 
                { 
                    GradebookEnrollmentId = x.GradebookEnrollmentId,
                    GradebookId = x.GradebookId,
                    ParticipantType = x.ParticipantTypes == null ? null : x.ParticipantTypes.ParticipantType,
                    Gradebook = x.Gradebooks!.GradeLevels == null ? null : x.Gradebooks.GradeLevels.GradeLevel,
                    Male = x.Male,
                    Female = x.Female,
                    Total = x.Male + x.Female
                })
                .OrderByDescending(x => x.ParticipantType)
                .ToListAsync();

            if (gradebookEnrollments == null)
            {
                return NotFound();
            }

            var model = new GradebookEnrollmentSummaryViewModel
            {
                InfoPanel = infoPanel,
                GradebookEnrollments = gradebookEnrollments
            };

            return View(model);
        }

        // GET: GradebookEnrollments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.GradebookEnrollments == null)
            {
                return NotFound();
            }

            var gradebookEnrollment = await _context.GradebookEnrollments
                .Include(g => g.Gradebooks)
                .Include(g => g.ParticipantTypes)
                .FirstOrDefaultAsync(m => m.GradebookEnrollmentId == id);
            if (gradebookEnrollment == null)
            {
                return NotFound();
            }

            return View(gradebookEnrollment);
        }

        // GET: GradebookEnrollments/Create
        public IActionResult Create()
        {
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId");
            ViewData["RefParticipantTypeId"] = new SelectList(_context.ParticipantTypes, "RefParticipantTypeId", "ParticipantType");
            return View();
        }

        // POST: GradebookEnrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradebookEnrollmentId,GradebookId,RegistrationDate,RefParticipantTypeId,Male,Female")] GradebookEnrollment gradebookEnrollment)
        {
            if (ModelState.IsValid)
            {
                gradebookEnrollment.GradebookEnrollmentId = Guid.NewGuid();
                _context.Add(gradebookEnrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId", gradebookEnrollment.GradebookId);
            ViewData["RefParticipantTypeId"] = new SelectList(_context.ParticipantTypes, "RefParticipantTypeId", "ParticipantType", gradebookEnrollment.RefParticipantTypeId);
            return View(gradebookEnrollment);
        }

        // GET: GradebookEnrollments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.GradebookEnrollments == null)
            {
                return NotFound();
            }

            var gradebookEnrollment = await _context.GradebookEnrollments
                .Where(g => g.GradebookEnrollmentId == id)
                .Include(g => g.Gradebooks)
                .ThenInclude(g => g.GradeLevels)
                .Include(g => g.ParticipantTypes)
                .FirstOrDefaultAsync();

                
            if (gradebookEnrollment == null)
            {
                return NotFound();
            }
            ViewData["GradebookId"] = gradebookEnrollment.GradebookId;
            ViewData["GradeLevel"] = gradebookEnrollment.Gradebooks.GradeLevels.GradeLevel;
            ViewData["ParticipantType"] = gradebookEnrollment.ParticipantTypes.ParticipantType;
            ViewData["RefParticipantTypeId"] = gradebookEnrollment.RefParticipantTypeId;
            ViewData["ParentId"] = gradebookEnrollment.GradebookId;

            return View(gradebookEnrollment);
        }

        // POST: GradebookEnrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("GradebookEnrollmentId,GradebookId,RegistrationDate,RefParticipantTypeId,Male,Female")] GradebookEnrollment gradebookEnrollment)
        {
            if (id != gradebookEnrollment.GradebookEnrollmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //search for existing gradebook where gradebookid is the same as the gradebookenrollment gradebookid
                    //and update IsMissingAssessments to true
                    Gradebook? Exists_Gradebook = await _context.Gradebooks
                        .Where(g => g.GradebookId == gradebookEnrollment.GradebookId)
                        .FirstOrDefaultAsync();

                    if (Exists_Gradebook != null)
                    {
                        Exists_Gradebook.IsMissingGradebookAssessments = false;
                    }


                    _context.Update(gradebookEnrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradebookEnrollmentExists(gradebookEnrollment.GradebookEnrollmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = gradebookEnrollment.GradebookId });
            }
            ViewData["GradebookId"] = new SelectList(_context.Gradebooks, "GradebookId", "GradebookId", gradebookEnrollment.GradebookId);
            ViewData["RefParticipantTypeId"] = new SelectList(_context.ParticipantTypes, "RefParticipantTypeId", "ParticipantType", gradebookEnrollment.RefParticipantTypeId);
            return View(gradebookEnrollment);
        }

        // GET: GradebookEnrollments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.GradebookEnrollments == null)
            {
                return NotFound();
            }

            var gradebookEnrollment = await _context.GradebookEnrollments
                .Include(g => g.Gradebooks)
                .Include(g => g.ParticipantTypes)
                .FirstOrDefaultAsync(m => m.GradebookEnrollmentId == id);
            if (gradebookEnrollment == null)
            {
                return NotFound();
            }

            return View(gradebookEnrollment);
        }

        // POST: GradebookEnrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.GradebookEnrollments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GradebookEnrollments'  is null.");
            }
            var gradebookEnrollment = await _context.GradebookEnrollments.FindAsync(id);
            if (gradebookEnrollment != null)
            {
                _context.GradebookEnrollments.Remove(gradebookEnrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradebookEnrollmentExists(Guid id)
        {
            return (_context.GradebookEnrollments?.Any(e => e.GradebookEnrollmentId == id)).GetValueOrDefault();
        }
    }
}
