using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HoneymoonShop.Data;
using HoneymoonShop.Models;

namespace HoneymoonShop.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeaturesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string name)
        {
            Feature f = new Feature() { Name = name };
            try
            {
                _context.Add(f);
                _context.SaveChanges();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Controlpanel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string name, int del)
        {
            Feature f = new Feature() { ID = id, Name = name };
            switch (del)
            {
                case 0:
                    break;

                case 1:
                    try
                    {
                        _context.Remove(f);
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
                _context.Update(f);
                _context.SaveChanges();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Controlpanel");
        }

        [HttpGet]
        public IActionResult Search(string feature)
        {
            List<Feature> f = _context.Feature.Where(feat => feat.Name.Contains(feature)).ToList();
            if (f == null) return RedirectToAction("Controlpanel", "Index");
            return View(f);
        }

        private bool FeatureExists(int id)
        {
            return _context.Feature.Any(e => e.ID == id);
        }
    }
}