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
    public class DressColorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DressColorsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: DressColors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DressColor.Include(d => d.Color).Include(d => d.Dress);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DressColors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressColor = await _context.DressColor.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressColor == null)
            {
                return NotFound();
            }

            return View(dressColor);
        }

        // GET: DressColors/Create
        public IActionResult Create()
        {
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID");
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID");
            return View();
        }

        // POST: DressColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DressID,ColorID")] DressColor dressColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dressColor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", dressColor.ColorID);
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressColor.DressID);
            return View(dressColor);
        }

        // GET: DressColors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressColor = await _context.DressColor.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressColor == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", dressColor.ColorID);
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressColor.DressID);
            return View(dressColor);
        }

        // POST: DressColors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DressID,ColorID")] DressColor dressColor)
        {
            if (id != dressColor.DressID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dressColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DressColorExists(dressColor.DressID))
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
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "ID", dressColor.ColorID);
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressColor.DressID);
            return View(dressColor);
        }

        // GET: DressColors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressColor = await _context.DressColor.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressColor == null)
            {
                return NotFound();
            }

            return View(dressColor);
        }

        // POST: DressColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dressColor = await _context.DressColor.SingleOrDefaultAsync(m => m.DressID == id);
            _context.DressColor.Remove(dressColor);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DressColorExists(int id)
        {
            return _context.DressColor.Any(e => e.DressID == id);
        }
    }
}
