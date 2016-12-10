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
    public class DressFeaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DressFeaturesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: DressFeatures
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DressFeature.Include(d => d.Dress).Include(d => d.Feature);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DressFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressFeature = await _context.DressFeature.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressFeature == null)
            {
                return NotFound();
            }

            return View(dressFeature);
        }

        // GET: DressFeatures/Create
        public IActionResult Create()
        {
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID");
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID");
            return View();
        }

        // POST: DressFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DressID,FeatureID")] DressFeature dressFeature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dressFeature);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressFeature.DressID);
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", dressFeature.FeatureID);
            return View(dressFeature);
        }

        // GET: DressFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressFeature = await _context.DressFeature.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressFeature == null)
            {
                return NotFound();
            }
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressFeature.DressID);
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", dressFeature.FeatureID);
            return View(dressFeature);
        }

        // POST: DressFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DressID,FeatureID")] DressFeature dressFeature)
        {
            if (id != dressFeature.DressID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dressFeature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DressFeatureExists(dressFeature.DressID))
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
            ViewData["DressID"] = new SelectList(_context.Dress, "ID", "ID", dressFeature.DressID);
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", dressFeature.FeatureID);
            return View(dressFeature);
        }

        // GET: DressFeatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dressFeature = await _context.DressFeature.SingleOrDefaultAsync(m => m.DressID == id);
            if (dressFeature == null)
            {
                return NotFound();
            }

            return View(dressFeature);
        }

        // POST: DressFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dressFeature = await _context.DressFeature.SingleOrDefaultAsync(m => m.DressID == id);
            _context.DressFeature.Remove(dressFeature);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DressFeatureExists(int id)
        {
            return _context.DressFeature.Any(e => e.DressID == id);
        }
    }
}
