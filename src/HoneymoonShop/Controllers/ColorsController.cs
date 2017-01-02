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
    public class ColorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColorsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Colors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Color.ToListAsync());
        }

        // POST: Colors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string color, string cod)
        {
            Color c = new Color() { Name = color, ColorCode = cod };
            try
            {
                _context.Add(c);
                _context.SaveChanges();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Controlpanel");
        }

        [HttpGet]
        public IActionResult Search(string color)
        {
            List<Color> c = _context.Color.Where(col => col.Name.Contains(color)).ToList();
            if (c == null) return RedirectToAction("Controlpanel", "Index");
            return View(c);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string color, string colorcod, int del)
        {
            Color c = new Color() { ID = id, ColorCode = colorcod, Name = color };
            switch (del)
            {
                case 0:
                    break;

                case 1:
                    try
                    {
                        _context.Remove(c);
                        _context.SaveChanges();
                    }
                    catch
                    {
                        return NotFound();
                    }
                    return RedirectToAction("Index", "Controlpanel");
            }
            try
            {
                _context.Update(c);
                _context.SaveChanges();
            } catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Controlpanel");
        }

        private bool ColorExists(int id)
        {
            return _context.Color.Any(e => e.ID == id);
        }
    }
}
