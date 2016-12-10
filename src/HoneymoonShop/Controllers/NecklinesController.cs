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
    public class NecklinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NecklinesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Necklines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Neckline.ToListAsync());
        }

        // GET: Necklines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neckline = await _context.Neckline.SingleOrDefaultAsync(m => m.ID == id);
            if (neckline == null)
            {
                return NotFound();
            }

            return View(neckline);
        }

        // GET: Necklines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Necklines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Neckline neckline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(neckline);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(neckline);
        }

        // GET: Necklines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neckline = await _context.Neckline.SingleOrDefaultAsync(m => m.ID == id);
            if (neckline == null)
            {
                return NotFound();
            }
            return View(neckline);
        }

        // POST: Necklines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Neckline neckline)
        {
            if (id != neckline.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(neckline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NecklineExists(neckline.ID))
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
            return View(neckline);
        }

        // GET: Necklines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var neckline = await _context.Neckline.SingleOrDefaultAsync(m => m.ID == id);
            if (neckline == null)
            {
                return NotFound();
            }

            return View(neckline);
        }

        // POST: Necklines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var neckline = await _context.Neckline.SingleOrDefaultAsync(m => m.ID == id);
            _context.Neckline.Remove(neckline);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NecklineExists(int id)
        {
            return _context.Neckline.Any(e => e.ID == id);
        }
    }
}
