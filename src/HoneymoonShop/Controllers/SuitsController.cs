using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HoneymoonShop.Controllers
{
    public class SuitsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}