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
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Reviews
        public async Task<IActionResult> Index(string r)
        {
            if (r!= null && r.Equals("pass")) ViewData["state"] = "Uw review is opgeslagen bedankt voor uw feedback!";
            else ViewData["state"] = "";
            //ViewData["state"] = await Message.Send("smtptestcore@gmail.com", "smtptestcore@gmail.com", "smtptestcore@gmail.com", "smtptestcore@gmail.com", "smtptestcore@gmail.com", "smtp.gmail.com", 587, "smtptestcore@gmail.com", "smtptestcore@gmail.com", "testcore", "smtptestcore@gmail.com", "smtptestcore@gmail.com", "smtptestcore@gmail.com");
            return View(await _context.Review.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["state"] = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(string name, string desc, int rat)
        {
            try
            {
                _context.Add(
                    new Review()
                    {
                        Date = DateTime.Now,
                        Description = desc,
                        Name = name,
                        Rating = rat
                    }
                );
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", new { r = "pass" });
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ID == id);
        }
    }
}
