using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GiftStore.Data;
using GiftStore.Models;

namespace GiftStore.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class TestimonialsController : Controller
    {
        private readonly MyDbContext _context;

        public TestimonialsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/Testimonials
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.testimonials.Include(t => t.User);
            return View(await myDbContext.ToListAsync());
        }

		public async Task<IActionResult> Approve(int id)
		{
			var testimonial = await _context.testimonials.FindAsync(id);
			if (testimonial == null)
			{
				return NotFound();
			}

			testimonial.IsApproved = true; // تعيين حالة الموافقة إلى true
			_context.Update(testimonial);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		// GET: Administrator/Testimonials/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: Administrator/Testimonials/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: Administrator/Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,DateAdded,UserId,IsApproved")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }

        // GET: Administrator/Testimonials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }

        // POST: Administrator/Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,DateAdded,UserId,IsApproved")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }

        // GET: Administrator/Testimonials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.testimonials
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Administrator/Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.testimonials == null)
            {
                return Problem("Entity set 'MyDbContext.testimonials'  is null.");
            }
            var testimonial = await _context.testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.testimonials.Remove(testimonial);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(int id)
        {
          return (_context.testimonials?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
