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
    public class ManusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManusController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Manus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Manu.ToListAsync());
        }

        // GET: Manus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manu = await _context.Manu.SingleOrDefaultAsync(m => m.ID == id);
            if (manu == null)
            {
                return NotFound();
            }

            return View(manu);
        }

        // GET: Manus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Manu manu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(manu);
        }

        // GET: Manus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manu = await _context.Manu.SingleOrDefaultAsync(m => m.ID == id);
            if (manu == null)
            {
                return NotFound();
            }
            return View(manu);
        }

        // POST: Manus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] Manu manu)
        {
            if (id != manu.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManuExists(manu.ID))
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
            return View(manu);
        }

        // GET: Manus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manu = await _context.Manu.SingleOrDefaultAsync(m => m.ID == id);
            if (manu == null)
            {
                return NotFound();
            }

            return View(manu);
        }

        // POST: Manus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manu = await _context.Manu.SingleOrDefaultAsync(m => m.ID == id);
            _context.Manu.Remove(manu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ManuExists(int id)
        {
            return _context.Manu.Any(e => e.ID == id);
        }
    }
}
