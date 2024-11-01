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
    public class GiftsController : Controller
    {
        private readonly MyDbContext _context;

        public GiftsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/Gifts
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.gifts.Include(g => g.Category);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Administrator/Gifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.gifts == null)
            {
                return NotFound();
            }

            var gift = await _context.gifts
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.GiftId == id);
            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // GET: Administrator/Gifts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name");
            return View();
        }

        // POST: Administrator/Gifts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GiftId,Name,Description,Price,ImageUrl,CategoryId,AvailableColors,discount,numberOfItems")] Gift gift , IFormFile GiftImg)
        {
            if (ModelState.IsValid)
            {

                if (GiftImg != null && GiftImg.Length > 0)
                {
                    var fileName = Path.GetFileName(GiftImg.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images1", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await GiftImg.CopyToAsync(fileStream);
                    }

                    gift.ImageUrl = "/images1/" + fileName;
                }
                else
                {
                    gift.ImageUrl = "/images1/default.jpg";
                }
                _context.Add(gift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", gift.CategoryId);
            return View(gift);
        }

        // GET: Administrator/Gifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.gifts == null)
            {
                return NotFound();
            }

            var gift = await _context.gifts.FindAsync(id);
            if (gift == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", gift.CategoryId);
            return View(gift);
        }

        // POST: Administrator/Gifts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GiftId,Name,Description,Price,ImageUrl,CategoryId,AvailableColors,discount,numberOfItems")] Gift gift , IFormFile GiftImg)
        {
            if (id != gift.GiftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (GiftImg != null && GiftImg.Length > 0)
                {
                    var fileName = Path.GetFileName(GiftImg.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images1", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await GiftImg.CopyToAsync(fileStream);
                    }

                    gift.ImageUrl = "/images1/" + fileName;
                }
                else
                {
                    gift.ImageUrl = "/images1/default.jpg";
                }
                try
                {
                    _context.Update(gift);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GiftExists(gift.GiftId))
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
            ViewData["CategoryId"] = new SelectList(_context.categories, "Id", "Name", gift.CategoryId);
            return View(gift);
        }

        // GET: Administrator/Gifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.gifts == null)
            {
                return NotFound();
            }

            var gift = await _context.gifts
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.GiftId == id);
            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // POST: Administrator/Gifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.gifts == null)
            {
                return Problem("Entity set 'MyDbContext.gifts'  is null.");
            }
            var gift = await _context.gifts.FindAsync(id);
            if (gift != null)
            {
                _context.gifts.Remove(gift);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GiftExists(int id)
        {
          return (_context.gifts?.Any(e => e.GiftId == id)).GetValueOrDefault();
        }
    }
}
