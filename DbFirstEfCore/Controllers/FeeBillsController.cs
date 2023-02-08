using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbFirstEfCore.Models;

namespace DbFirstEfCore.Controllers
{
    public class FeeBillsController : Controller
    {
        private readonly StudentDbContext _context;

        public FeeBillsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: FeeBills1
        public async Task<IActionResult> Index()
        {
            var studentDbContext = _context.FeeBills.Include(f => f.Student);
            return View(await studentDbContext.ToListAsync());
        }

        // GET: FeeBills1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FeeBills == null)
            {
                return NotFound();
            }

            var feeBill = await _context.FeeBills
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feeBill == null)
            {
                return NotFound();
            }

            return View(feeBill);
        }

        // GET: FeeBills1/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: FeeBills1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,BillingMonth,PreviousArrears,TutionFee,AdmissionFee,Fine,StationaryCharges,FeePayable,FeePaid,NextArrears")] FeeBill feeBill)
        {
            feeBill.FeePayable = (decimal)(feeBill.AdmissionFee
                                           + feeBill.Fine
                                           + feeBill.PreviousArrears
                                           + feeBill.StationaryCharges
                                           + feeBill.TutionFee);

            //feeBill.FeePayable = 0;
            feeBill.NextArrears = 0;
            feeBill.FeePaid = 0;
            feeBill.Student = _context.Students.FirstOrDefault(x => x.Id == feeBill.StudentId);

            try
            {
                //if (ModelState.IsValid)
                {
                    _context.Add(feeBill);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", feeBill.StudentId);
                return View(feeBill);
            }
        }

        // GET: FeeBills1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FeeBills == null)
            {
                return NotFound();
            }

            var feeBill = await _context.FeeBills.FindAsync(id);
            if (feeBill == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", feeBill.StudentId);
            return View(feeBill);
        }

        // POST: FeeBills1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,BillingMonth,PreviousArrears,TutionFee,AdmissionFee,Fine,StationaryCharges,FeePayable,FeePaid,NextArrears")] FeeBill feeBill)
        {
            if (id != feeBill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feeBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeeBillExists(feeBill.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", feeBill.StudentId);
            return View(feeBill);
        }

        // GET: FeeBills1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FeeBills == null)
            {
                return NotFound();
            }

            var feeBill = await _context.FeeBills
                .Include(f => f.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feeBill == null)
            {
                return NotFound();
            }

            return View(feeBill);
        }

        // POST: FeeBills1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FeeBills == null)
            {
                return Problem("Entity set 'StudentDbContext.FeeBills'  is null.");
            }
            var feeBill = await _context.FeeBills.FindAsync(id);
            if (feeBill != null)
            {
                _context.FeeBills.Remove(feeBill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeeBillExists(int id)
        {
            return _context.FeeBills.Any(e => e.Id == id);
        }
    }
}
