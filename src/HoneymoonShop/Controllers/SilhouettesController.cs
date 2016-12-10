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
    public class SilhouettesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SilhouettesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Silhouettes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Silhouette.ToListAsync());
        }

        // GET: Silhouettes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var silhouette = await _context.Silhouette.SingleOrDefaultAsync(m => m.ID == id);
            if (silhouette == null)
            {
                return NotFound();
            }

            return View(silhouette);
        }

        // GET: Silhouettes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Silhouettes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Silhouette silhouette)
        {
            if (ModelState.IsValid)
            {
                _context.Add(silhouette);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(silhouette);
        }

        // GET: Silhouettes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var silhouette = await _context.Silhouette.SingleOrDefaultAsync(m => m.ID == id);
            if (silhouette == null)
            {
                return NotFound();
            }
            return View(silhouette);
        }

        // POST: Silhouettes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Silhouette silhouette)
        {
            if (id != silhouette.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(silhouette);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SilhouetteExists(silhouette.ID))
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
            return View(silhouette);
        }

        // GET: Silhouettes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var silhouette = await _context.Silhouette.SingleOrDefaultAsync(m => m.ID == id);
            if (silhouette == null)
            {
                return NotFound();
            }

            return View(silhouette);
        }

        // POST: Silhouettes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var silhouette = await _context.Silhouette.SingleOrDefaultAsync(m => m.ID == id);
            _context.Silhouette.Remove(silhouette);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SilhouetteExists(int id)
        {
            return _context.Silhouette.Any(e => e.ID == id);
        }
    }
}
