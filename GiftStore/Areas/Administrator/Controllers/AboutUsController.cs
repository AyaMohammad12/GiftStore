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
    public class AboutUsController : Controller
    {
        private readonly MyDbContext _context;

        public AboutUsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/AboutUs
        public async Task<IActionResult> Index()
        {
              return _context.aboutUs != null ? 
                          View(await _context.aboutUs.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.aboutUs'  is null.");
        }

        // GET: Administrator/AboutUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.aboutUs == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUs == null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        // GET: Administrator/AboutUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/AboutUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,txt")] AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aboutUs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aboutUs);
        }

        // GET: Administrator/AboutUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.aboutUs == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs.FindAsync(id);
            if (aboutUs == null)
            {
                return NotFound();
            }
            return View(aboutUs);
        }

        // POST: Administrator/AboutUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,txt")] AboutUs aboutUs)
        {
            if (id != aboutUs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutUs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsExists(aboutUs.Id))
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
            return View(aboutUs);
        }

        // GET: Administrator/AboutUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.aboutUs == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.aboutUs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aboutUs == null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        // POST: Administrator/AboutUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.aboutUs == null)
            {
                return Problem("Entity set 'MyDbContext.aboutUs'  is null.");
            }
            var aboutUs = await _context.aboutUs.FindAsync(id);
            if (aboutUs != null)
            {
                _context.aboutUs.Remove(aboutUs);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutUsExists(int id)
        {
          return (_context.aboutUs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
