using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneymoonShop.Models;
using HoneymoonShop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HoneymoonShop.Controllers
{
    [Authorize]
    public class ControlpanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;

        public ControlpanelController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(int art)
        {
            Dress dress = _context.Dress
                .Include(d => d.Category)
                .Include(d => d.Manu)
                .Include(d => d.Neckline)
                .Include(d => d.Silhouette)
                .Include(d => d.Style)
                .Where(d => d.ID == art)
                .First();

            List<string> files = new List<string>();
            string path = Path.Combine(_env.WebRootPath, $"images/dress/{dress.ID}");
            foreach (string s in Directory.GetFiles(path))
            {
                string filename = s.Replace(path + "\\", string.Empty);
                files.Add($"{dress.ID}/" + filename);
            }

            ViewData["image"] = files;
            ViewData["cat"] = _context.Category.ToList();
            ViewData["manu"] = _context.Manu.ToList();
            ViewData["neck"] = _context.Neckline.ToList();
            ViewData["sil"] = _context.Silhouette.ToList();
            ViewData["style"] = _context.Style.ToList();
            ViewData["color"] = _context.Color.ToList();
            ViewData["feature"] = _context.Feature.ToList();
            ViewData["dresscolor"] = _context.DressColor.ToList();
            ViewData["dressfeature"] = _context.DressFeature.ToList();

            return View(dress);
        }

        [HttpGet]
        public IActionResult SearchBrand(string s)
        {
            List<Dress> dresses = _context.Dress.Include(d => d.Manu).Where(d => d.Manu.Name.Contains(s)).ToList();
            return View();
        }
    }
}