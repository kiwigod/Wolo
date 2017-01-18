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
    public class SuitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuitsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Suits
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Suit.Include(s => s.Category).Include(s => s.Manu).Include(s => s.Style);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Suits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suit = await _context.Suit.SingleOrDefaultAsync(m => m.ID == id);
            if (suit == null)
            {
                return NotFound();
            }

            return View(suit);
        }

        // GET: Suits/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID");
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID");
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID");
            return View();
        }

        // POST: Suits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,Description,ManuID,Price,StyleID")] Suit suit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suit);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", suit.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", suit.ManuID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", suit.StyleID);
            return View(suit);
        }

        // GET: Suits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suit = await _context.Suit.SingleOrDefaultAsync(m => m.ID == id);
            if (suit == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", suit.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", suit.ManuID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", suit.StyleID);
            return View(suit);
        }

        // POST: Suits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,Description,ManuID,Price,StyleID")] Suit suit)
        {
            if (id != suit.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuitExists(suit.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", suit.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", suit.ManuID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", suit.StyleID);
            return View(suit);
        }

        // GET: Suits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suit = await _context.Suit.SingleOrDefaultAsync(m => m.ID == id);
            if (suit == null)
            {
                return NotFound();
            }

            return View(suit);
        }

        // POST: Suits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suit = await _context.Suit.SingleOrDefaultAsync(m => m.ID == id);
            _context.Suit.Remove(suit);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SuitExists(int id)
        {
            return _context.Suit.Any(e => e.ID == id);
        }
    }
}
