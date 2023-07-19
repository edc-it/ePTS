using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ePTS.Data;
using ePTS.Entities.Assessments;
using Microsoft.AspNetCore.Authorization;
// remove
namespace ePTS.Web.Controllers
{
    [Authorize]
    public class AssessmentPerformanceLevelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssessmentPerformanceLevelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AssessmentPerformanceLevels
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null || _context.AssessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var applicationDbContext = _context.AssessmentPerformanceLevels
                .Where(x => x.GradebookAssessmentId == id)
                .Include(a => a.GradebookAssessments)
                .Include(a => a.PerformanceLevels);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AssessmentPerformanceLevels/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.AssessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var assessmentPerformanceLevel = await _context.AssessmentPerformanceLevels
                .Include(a => a.GradebookAssessments)
                .Include(a => a.PerformanceLevels)
                .FirstOrDefaultAsync(m => m.AssessmentPerformanceLevelId == id);
            if (assessmentPerformanceLevel == null)
            {
                return NotFound();
            }

            return View(assessmentPerformanceLevel);
        }

        // GET: AssessmentPerformanceLevels/Create
        public IActionResult Create()
        {
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId");
            ViewData["RefPerformanceLevelId"] = new SelectList(_context.PerformanceLevels, "RefPerformanceLevelId", "PerformanceLevel");
            return View();
        }

        // POST: AssessmentPerformanceLevels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssessmentPerformanceLevelId,GradebookAssessmentId,RegistrationDate,RefPerformanceLevelId,RefSexId,PossibleValue,ScoreValue")] AssessmentPerformanceLevel assessmentPerformanceLevel)
        {
            if (ModelState.IsValid)
            {
                assessmentPerformanceLevel.AssessmentPerformanceLevelId = Guid.NewGuid();
                _context.Add(assessmentPerformanceLevel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId", assessmentPerformanceLevel.GradebookAssessmentId);
            ViewData["RefPerformanceLevelId"] = new SelectList(_context.PerformanceLevels, "RefPerformanceLevelId", "PerformanceLevel", assessmentPerformanceLevel.RefPerformanceLevelId);
            return View(assessmentPerformanceLevel);
        }

        // GET: AssessmentPerformanceLevels/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.AssessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var assessmentPerformanceLevel = await _context.AssessmentPerformanceLevels.FindAsync(id);
            if (assessmentPerformanceLevel == null)
            {
                return NotFound();
            }
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId", assessmentPerformanceLevel.GradebookAssessmentId);
            ViewData["RefPerformanceLevelId"] = new SelectList(_context.PerformanceLevels, "RefPerformanceLevelId", "PerformanceLevel", assessmentPerformanceLevel.RefPerformanceLevelId);
            return View(assessmentPerformanceLevel);
        }

        // POST: AssessmentPerformanceLevels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AssessmentPerformanceLevelId,GradebookAssessmentId,RegistrationDate,RefPerformanceLevelId,RefSexId,PossibleValue,ScoreValue")] AssessmentPerformanceLevel assessmentPerformanceLevel)
        {
            if (id != assessmentPerformanceLevel.AssessmentPerformanceLevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assessmentPerformanceLevel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessmentPerformanceLevelExists(assessmentPerformanceLevel.AssessmentPerformanceLevelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradebookAssessmentId"] = new SelectList(_context.GradebookAssessments, "GradebookAssessmentId", "GradebookAssessmentId", assessmentPerformanceLevel.GradebookAssessmentId);
            ViewData["RefPerformanceLevelId"] = new SelectList(_context.PerformanceLevels, "RefPerformanceLevelId", "PerformanceLevel", assessmentPerformanceLevel.RefPerformanceLevelId);
            return View(assessmentPerformanceLevel);
        }

        // GET: AssessmentPerformanceLevels/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.AssessmentPerformanceLevels == null)
            {
                return NotFound();
            }

            var assessmentPerformanceLevel = await _context.AssessmentPerformanceLevels
                .Include(a => a.GradebookAssessments)
                .Include(a => a.PerformanceLevels)
                .FirstOrDefaultAsync(m => m.AssessmentPerformanceLevelId == id);
            if (assessmentPerformanceLevel == null)
            {
                return NotFound();
            }

            return View(assessmentPerformanceLevel);
        }

        // POST: AssessmentPerformanceLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.AssessmentPerformanceLevels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AssessmentPerformanceLevels'  is null.");
            }
            var assessmentPerformanceLevel = await _context.AssessmentPerformanceLevels.FindAsync(id);
            if (assessmentPerformanceLevel != null)
            {
                _context.AssessmentPerformanceLevels.Remove(assessmentPerformanceLevel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessmentPerformanceLevelExists(Guid id)
        {
          return (_context.AssessmentPerformanceLevels?.Any(e => e.AssessmentPerformanceLevelId == id)).GetValueOrDefault();
        }
    }
}
