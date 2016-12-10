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
    public class DressesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DressesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Dresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dress.Include(d => d.Color).Include(d => d.Manu).Include(d => d.Neckline).Include(d => d.Silhouette).Include(d => d.Style);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            if (dress == null)
            {
                return NotFound();
            }

            return View(dress);
        }

        // GET: Dresses/Create
        public IActionResult Create()
        {
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "ID");
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "ID");
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "ID");
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "ID");
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "ID");
            return View();
        }

        // POST: Dresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ColorID,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "ID", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "ID", dress.StyleID);
            return View(dress);
        }

        // GET: Dresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            if (dress == null)
            {
                return NotFound();
            }
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "ID", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "ID", dress.StyleID);
            return View(dress);
        }

        // POST: Dresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ColorID,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (id != dress.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DressExists(dress.ID))
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
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "ID", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "ID", dress.StyleID);
            return View(dress);
        }

        // GET: Dresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            if (dress == null)
            {
                return NotFound();
            }

            return View(dress);
        }

        // POST: Dresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            _context.Dress.Remove(dress);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DressExists(int id)
        {
            return _context.Dress.Any(e => e.ID == id);
        }
    }
}
