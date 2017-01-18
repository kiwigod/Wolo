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
    public class SuitFeaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuitFeaturesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SuitFeatures
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuitFeature.Include(s => s.Feature).Include(s => s.Suit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SuitFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitFeature = await _context.SuitFeature.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitFeature == null)
            {
                return NotFound();
            }

            return View(suitFeature);
        }

        // GET: SuitFeatures/Create
        public IActionResult Create()
        {
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID");
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID");
            return View();
        }

        // POST: SuitFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SuitID,FeatureID")] SuitFeature suitFeature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suitFeature);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", suitFeature.FeatureID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitFeature.SuitID);
            return View(suitFeature);
        }

        // GET: SuitFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitFeature = await _context.SuitFeature.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitFeature == null)
            {
                return NotFound();
            }
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", suitFeature.FeatureID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitFeature.SuitID);
            return View(suitFeature);
        }

        // POST: SuitFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SuitID,FeatureID")] SuitFeature suitFeature)
        {
            if (id != suitFeature.SuitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suitFeature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuitFeatureExists(suitFeature.SuitID))
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
            ViewData["FeatureID"] = new SelectList(_context.Feature, "ID", "ID", suitFeature.FeatureID);
            ViewData["SuitID"] = new SelectList(_context.Suit, "ID", "ID", suitFeature.SuitID);
            return View(suitFeature);
        }

        // GET: SuitFeatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suitFeature = await _context.SuitFeature.SingleOrDefaultAsync(m => m.SuitID == id);
            if (suitFeature == null)
            {
                return NotFound();
            }

            return View(suitFeature);
        }

        // POST: SuitFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suitFeature = await _context.SuitFeature.SingleOrDefaultAsync(m => m.SuitID == id);
            _context.SuitFeature.Remove(suitFeature);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SuitFeatureExists(int id)
        {
            return _context.SuitFeature.Any(e => e.SuitID == id);
        }
    }
}
