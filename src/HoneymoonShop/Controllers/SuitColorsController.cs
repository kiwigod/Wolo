using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoneymoonShop.Data;
using HoneymoonShop.Models;

namespace HoneymoonShop.Controllers
{
    public class SuitColorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuitColorsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SuitColors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuitColor.Include(s => s.Color).Include(s => s.Suit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuitColors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitColor = await _context.SuitColor.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitColor == null)
            {
                return NotFound();
            }

            return View(suitColor);
        }

        // GET: SuitColors/Create
        public IActionResult Create()
        {
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID");
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID");
            return View();
        }

        // POST: SuitColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SuitID,ColorID")] SuitColor suitColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suitColor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", suitColor.ColorID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitColor.SuitID);
            return View(suitColor);
        }

        // GET: SuitColors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitColor = await _context.SuitColor.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitColor == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", suitColor.ColorID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitColor.SuitID);
            return View(suitColor);
        }

        // POST: SuitColors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SuitID,ColorID")] SuitColor suitColor)
        {
            if (id != suitColor.SuitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suitColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuitColorExists(suitColor.SuitID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", suitColor.ColorID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitColor.SuitID);
            return View(suitColor);
        }

        // GET: SuitColors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitColor = await _context.SuitColor.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitColor == null)
            {
                return NotFound();
            }

            return View(suitColor);
        }

        // POST: SuitColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suitColor = await _context.SuitColor.SingleOrDefaultAsync(m => m.SuitID == id);
            _context.SuitColor.Remove(suitColor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SuitColorExists(int id)
        {
            return _context.SuitColor.Any(e => e.SuitID == id);
        }
    }
}
