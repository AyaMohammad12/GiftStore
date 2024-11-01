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
    public class SlidersController : Controller
    {
        private readonly MyDbContext _context;

        public SlidersController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/Sliders
        public async Task<IActionResult> Index()
        {
              return _context.sliders != null ? 
                          View(await _context.sliders.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.sliders'  is null.");
        }

        // GET: Administrator/Sliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders
                .FirstOrDefaultAsync(m => m.SliderId == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // GET: Administrator/Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SliderId,SliderImg1,SliderImg2,SliderImg3")] Slider slider, IFormFile SliderImg)
        {
            if (ModelState.IsValid)
            {
                if (SliderImg != null && SliderImg.Length > 0)
                {
                    var fileName = Path.GetFileName(SliderImg.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images1", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await SliderImg.CopyToAsync(fileStream);
                    }

                    slider.SliderImg1 = "/images1/" + fileName;
                }
                else
                {
                    slider.SliderImg1 = "/images1/default.jpg";
                }
                _context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        // GET: Administrator/Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        // POST: Administrator/Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SliderId,SliderImg1,SliderImg2,SliderImg3")] Slider slider , IFormFile SliderImg)
        {
            if (id != slider.SliderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (SliderImg != null && SliderImg.Length > 0)
                {
                    var fileName = Path.GetFileName(SliderImg.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images1", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await SliderImg.CopyToAsync(fileStream);
                    }

                    slider.SliderImg1 = "/images1/" + fileName;
                }
                else
                {
                    slider.SliderImg1 = "/images1/default.jpg";
                }
                try
                {
                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.SliderId))
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
            return View(slider);
        }

        // GET: Administrator/Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sliders == null)
            {
                return NotFound();
            }

            var slider = await _context.sliders
                .FirstOrDefaultAsync(m => m.SliderId == id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        // POST: Administrator/Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sliders == null)
            {
                return Problem("Entity set 'MyDbContext.sliders'  is null.");
            }
            var slider = await _context.sliders.FindAsync(id);
            if (slider != null)
            {
                _context.sliders.Remove(slider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
          return (_context.sliders?.Any(e => e.SliderId == id)).GetValueOrDefault();
        }
    }
}
