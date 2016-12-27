using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneymoonShop.Models;
using HoneymoonShop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace HoneymoonShop.Controllers
{
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
            List<Dress> dress = _context.Dress
                .Include(d => d.Category)
                .Include(d => d.Manu)
                .Include(d => d.Neckline)
                .Include(d => d.Silhouette)
                .Include(d => d.Style)
                .ToList();
                
            return View(dress);
        }
    }
}