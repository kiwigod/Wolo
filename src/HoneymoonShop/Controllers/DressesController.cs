using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HoneymoonShop.Data;
using HoneymoonShop.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace HoneymoonShop.Controllers
{
    public class DressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;
        private List<Dress> dresses;
        private int lastMin;
        private int lastMax;

        public DressesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
            dresses = new List<Dress>();
            lastMax = -1;
            lastMin = -1;
        }

        // GET: Dresses
        public IActionResult Index()
        {
            return View();
        }

        // GET: Dresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress
                .Include(d => d.Manu)
                .Include(d => d.DressColors)
                .Include(d => d.DressFeatures)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (dress == null)
            {
                return NotFound();
            }

            string path = Path.Combine(_env.WebRootPath, $"images/dress/{id}");
            DirectoryInfo di = new DirectoryInfo(path);
            List<string> images = new List<string>();
            foreach (string s in Directory.GetFiles(path))
            {
                string filename = s.Replace(path + "\\", string.Empty);
                images.Add($"{id}/" + filename);
            }

            List<string> colors = new List<string>();
            foreach (DressColor dc in dress.DressColors)
            {
                colors.Add(_context.Color.Where(c => dc.ColorID == c.ID).First().Name);
            }

            List<string> features = new List<string>();
            foreach (DressFeature df in dress.DressFeatures)
            {
                features.Add(_context.Feature.Where(f => df.FeatureID == f.ID).First().Name);
            }

            //Recommendend dresses
            List<Dress> recDress = new List<Dress>();
            foreach (Dress d in _context.Dress.Include(d => d.Manu))
            {
                if (d.Equals(dress)) { }
                else if (d.ManuID == dress.ManuID) recDress.Add(d);
                if (recDress.Count >= 4) break;
            }
            if (recDress.Count <= 4)
            {
                foreach (Dress d in _context.Dress.Include(d => d.Manu))
                {
                    if (d.Equals(dress)) { }
                    else if (d.StyleID == dress.StyleID && !recDress.Contains(d)) recDress.Add(d);
                    if (recDress.Count >= 4) break;
                }
            }

            var recImg = new Dictionary<int, string>();
            foreach (Dress d in recDress)
            {
                path = Path.Combine(_env.WebRootPath, $"images/dress/{d.ID}");
                di = new DirectoryInfo(path);
                foreach (string file in Directory.GetFiles(path))
                {
                    string filename = file.Replace(path + "\\", string.Empty);
                    if (filename.StartsWith("1"))
                    {
                        recImg.Add(d.ID, $"{d.ID}/" + filename);
                    }
                }
            }

            ViewData["RecDress"] = recDress;
            ViewData["RecImages"] = recImg;
            ViewData["Features"] = features;
            ViewData["Colors"] = colors;
            ViewData["Images"] = images;

            return View(dress);
        }

        // GET: Dresses/Create
        public IActionResult Create(int stage)
        {
            switch (stage)
            {
                case 2:
                    ViewData["cat"] = new SelectList(_context.Category.OrderBy(c => c.Name).ToList(), "ID", "Name");
                    ViewData["manu"] = new SelectList(_context.Manu.OrderBy(m => m.Name), "ID", "Name");
                    ViewData["neck"] = new SelectList(_context.Neckline.OrderBy(n => n.Name), "ID", "Name");
                    ViewData["sil"] = new SelectList(_context.Silhouette.OrderBy(s => s.Name), "ID", "Name");
                    ViewData["style"] = new SelectList(_context.Style.OrderBy(s => s.Name), "ID", "Name");
                    ViewData["color"] = _context.Color.ToList();
                    ViewData["feature"] = _context.Feature.ToList();
                    List<string> files = new List<string>();
                    Dress dress = _context.Dress.Where(d => d.Description == "Temp").First();
                    string path = Path.Combine(_env.WebRootPath, $"images/dress/{dress.ID}");
                    foreach (string s in Directory.GetFiles(path))
                    {
                        string filename = s.Replace(path + "\\", string.Empty);
                        files.Add($"{dress.ID}/" + filename);
                    }
                    ViewData["image"] = files;
                    return View(dress);

                default:
                    return RedirectToAction("AddImage");
            }
        }

        // GET: Dresses/Overview
        public IActionResult Overview()
        {
            List<Dress> newDress = _context.Dress
                .ToList();
            newDress.Sort((x, y) => y.ID.CompareTo(x.ID));
            newDress = newDress.Take(6).ToList();

            var img = new Dictionary<int, string>();
            foreach (Dress d in newDress)
            {
                string path = Path.Combine(_env.WebRootPath, $"images/dress/{d.ID}");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (string s in Directory.GetFiles(path))
                {
                    string filename = s.Replace(path + "\\", string.Empty);
                    if (filename.StartsWith("1")) img.Add(d.ID, $"{d.ID}/" + filename);
                }
            }

            ViewData["Images"] = img;
            ViewData["CategoryID"] = _context.Category.ToList();
            ViewData["ColorID"] = _context.Color.ToList();
            ViewData["ManuID"] = _context.Manu.ToList();
            ViewData["NecklineID"] = _context.Neckline.ToList();
            ViewData["SilhouetteID"] = _context.Silhouette.ToList();
            ViewData["StyleID"] = _context.Style.ToList();
            ViewData["DressMax"] = _context.Dress.Max(d => d.Price);
            return View(newDress);
            //return RedirectToAction("OverviewFiltered", "Dresses", new
            //{
            //    manu = new string[1] { "all" },
            //    style = new string[1] { "all" },
            //    pricemin = 0,
            //    pricemax = _context.Dress.Max(d => d.Price),
            //    neckline = new string[1] { "all" },
            //    silhouette = new string[1] { "all" },
            //    color = new string[1] { "all" },
            //    sort = "alf",
            //    amnt = 24,
            //    cat = 1,
            //    page = 1
            //});
        }

        [HttpGet]
        public IActionResult OverviewFiltered(string[] manu, string[] style, int pricemin, int pricemax, string[] neckline, string[] silhouette, string[] color, int request, string sort, int amnt, string cat, int page)
        {
            if (dresses.Count < 1 || pricemin != lastMin || pricemax != lastMax)
            {
                dresses = _context.Dress
                    .Include(d => d.Manu)
                    .Include(d => d.DressColors)
                    .Where(d => d.Price >= pricemin && d.Price <= pricemax)
                    .Where(d => manu.Contains(d.ManuID.ToString()) || manu.Contains("all"))
                    .Where(d => style.Contains(d.StyleID.ToString()) || style.Contains("all"))
                    .Where(d => neckline.Contains(d.NecklineID.ToString()) || neckline.Contains("all"))
                    .Where(d => silhouette.Contains(d.SilhouetteID.ToString()) || silhouette.Contains("all"))
                    .Where(d => d.DressColors.Where(dc => dc.DressID == d.ID).Any(dc => color.Contains(dc.ColorID.ToString())) || color.Contains("all"))
                    .Where(d => cat.Equals(d.CategoryID.ToString()) || cat.Equals("all"))
                    .ToList();
            }

            List<Dress> displayedDress = new List<Dress>();
            if (page != 1)
            {
                displayedDress = dresses.Skip(page - 1 * amnt).Take(amnt).ToList();
            }
            else
            {
                displayedDress = dresses.Take(amnt).ToList();
            }

            var img = new Dictionary<int, string>();
            foreach (Dress d in displayedDress)
            {
                string path = Path.Combine(_env.WebRootPath, $"images/dress/{d.ID}");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (string s in Directory.GetFiles(path))
                {
                    string filename = s.Replace(path + "\\", string.Empty);
                    if (filename.StartsWith("1")) img.Add(d.ID, $"{d.ID}/" + filename);
                }
            }

            switch (sort)
            {
                case "hl":
                    displayedDress.Sort((x, y) => y.Price.CompareTo(x.Price));
                    break;

                case "lh":
                    displayedDress.Sort((x, y) => x.Price.CompareTo(y.Price));
                    break;

                case "alf":
                    displayedDress.Sort((x, y) => y.Manu.Name.CompareTo(x.Manu.Name));
                    break;

                case "new":
                    displayedDress.Sort((x, y) => y.ID.CompareTo(x.ID));
                    break;
            }

            ViewData["CurrentPage"] = page;
            ViewData["Pages"] = (int)Math.Ceiling((double)dresses.Count / 10);
            ViewData["Images"] = img;
            ViewData["MinPrice"] = pricemin;
            ViewData["Maxprice"] = pricemax;
            ViewData["DressMax"] = _context.Dress.Max(d => d.Price);
            if (request == 1) return PartialView("OverviewPartial", displayedDress);
            else
            {
                ViewData["CategoryID"] = _context.Category.ToList();
                ViewData["ColorID"] = _context.Color.ToList();
                ViewData["Colorcheck"] = check(color);
                ViewData["ManuID"] = _context.Manu.ToList();
                ViewData["Manucheck"] = check(manu);
                ViewData["NecklineID"] = _context.Neckline.ToList();
                ViewData["Neckcheck"] = check(neckline);
                ViewData["SilhouetteID"] = _context.Silhouette.ToList();
                ViewData["Silcheck"] = check(silhouette);
                ViewData["StyleID"] = _context.Style.ToList();
                ViewData["Stylecheck"] = check(style);
                return View();
            }
        }

        private Dictionary<int, int> check(string[] array)
        {
            Dictionary<int, int> arraycheck = new Dictionary<int, int>();
            foreach(string s in array)
            {
                if (s.Equals("all")) break;
                else
                {
                    arraycheck.Add(int.Parse(s), 1);
                }
            }
            return arraycheck;
        }

        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(ICollection<IFormFile> images)
        {
            try
            {
                _context.Remove(_context.Dress.Where(d => d.Description.Equals("Temp")).First());
                await _context.SaveChangesAsync();
            }
            catch { }
            _context.Add(new Dress() { CategoryID = 1, ManuID = 1, NecklineID = 1, SilhouetteID = 1, StyleID = 1, Description = "Temp" });
            _context.SaveChanges();

            Dress temp = _context.Dress.Where(d => d.Description == "Temp").First();
            var uploads = Path.Combine(_env.WebRootPath, $"images/dress/{temp.ID}");
            Directory.CreateDirectory(uploads);
            string[] accepted = { "jpg", "jpeg", "png", "webp", "bmp" };

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    string[] filename = image.FileName.Split('.');
                    if (accepted.Contains(filename[filename.Length-1].ToLower()))
                    {
                        List<string> imgs = new List<string>();
                        foreach (string img in Directory.GetFiles(uploads))
                        {
                            string imgname = img.Replace(uploads + "\\", string.Empty);
                            string[] imgdet = imgname.Split('.');
                            imgs.Add(imgdet[0]);
                        }
                        if (Directory.GetFiles(uploads).Length == 0)
                        {

                            using (var stream = new FileStream(Path.Combine(uploads, $"1.{filename[filename.Length - 1]}"), FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }
                        }
                        else
                        {
                            string name = $"{(int.Parse(imgs[imgs.Count - 1]) + 1).ToString() + "." + filename[filename.Length - 1]}";
                            using (var stream = new FileStream(Path.Combine(uploads, name), FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }
                        }
                    }
                }
            }
            //Thread.Sleep(1000);
            int id = _context.Dress.Where(d => d.Description.Equals("Temp")).First().ID;
            return RedirectToAction("Create", new { stage = 2 });
        }

        public IActionResult AddColor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = _context.Dress.Where(d => d.ID == id).First();
            if (dress == null)
            {
                return NotFound();
            }

            List<Color> currentColors = new List<Color>();
            List<Color> availableColors = new List<Color>();
            foreach (DressColor dc in _context.DressColor)
            {
                if (dc.DressID == id)
                {
                    currentColors.Add(_context.Color.Where(c => c.ID == dc.ColorID).First());
                }
            }
            foreach (Color c in _context.Color)
            {
                if (currentColors.Find(cc => cc.ID == c.ID) == null)
                {
                    availableColors.Add(c);
                }
            }

            List<string> currentCName = new List<string>();
            foreach (Color c in currentColors)
            {
                currentCName.Add(c.Name);
            }

            ViewData["CurrentColors"] = currentCName;
            ViewData["AvailableColors"] = new SelectList(availableColors, "ID", "Name");
            List<Dress> temp = new List<Dress>();
            temp.Add(dress);
            ViewData["Dress"] = new SelectList(temp, "ID", "ID");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddColor(int? id, [Bind("DressID,ColorID")] DressColor dc)
        {
            if (id != dc.DressID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(dc);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddColor", dc.DressID);
            }
            return NotFound();
        }

        // GET: Dresses/AddFeature/5
        // Add features to a specific dress (maybe replace function to DressFeaturesController?)
        public IActionResult AddFeature(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = _context.Dress.Where(d => d.ID == id).First();
            if (dress == null)
            {
                return NotFound();
            }

            List<Feature> CurrentFeatures = new List<Feature>();
            List<Feature> AvailableFeatures = new List<Feature>();
            foreach (DressFeature df in _context.DressFeature)
            {
                if (df.DressID == id)
                {
                    CurrentFeatures.Add(_context.Feature.Where(f => f.ID == df.FeatureID).First());
                }
            }
            foreach (Feature f in _context.Feature)
            {
                if (CurrentFeatures.Find(cf => cf.ID == f.ID) == null)
                {
                    AvailableFeatures.Add(f);
                }
            }

            List<string> CurrentFName = new List<string>();
            foreach (Feature f in CurrentFeatures)
            {
                CurrentFName.Add(f.Name);
            }

            ViewData["CurrentFeatures"] = CurrentFName;
            ViewData["AvailableFeatures"] = new SelectList(AvailableFeatures, "ID", "Name");
            List<Dress> temp = new List<Dress>();
            temp.Add(dress);
            ViewData["Dress"] = new SelectList(temp, "ID", "ID");

            return View();
        }

        // POST: Dresses/AddFeature/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFeature(int? id, [Bind("DressID,FeatureID")] DressFeature df)
        {
            if (id != df.DressID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(df);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddFeature", df.DressID);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, int cat, string desc, int manu, int neck, int price, int sil, int style, int[] color, int[] feature, int[] image)
        {
            Dress d = new Dress() { ID = id, CategoryID = cat, Description = desc, ManuID = manu, NecklineID = neck, Price = price, SilhouetteID = sil, StyleID = style };
            _context.Update(d);

            List<DressColor> currentCol = _context.DressColor.Where(dc => dc.DressID == id).ToList();
            List<DressFeature> currentFeat = _context.DressFeature.Where(df => df.DressID == id).ToList();
            foreach (var dc in currentCol)
            {
                _context.Remove(dc);
            }
            foreach (var df in currentFeat)
            {
                _context.Remove(df);
            }
            _context.SaveChanges();

            foreach (int c in color)
            {
                _context.Add(new DressColor() { DressID = id, ColorID = c });
            }
            foreach (int f in feature)
            {
                _context.Add(new DressFeature() { DressID = id, FeatureID = f });
            }
            _context.SaveChanges();

            string path = Path.Combine(_env.WebRootPath, $"images/dress/{id}");
            int counter = 1;
            foreach (string photo in Directory.GetFiles(path))
            {
                string[] filename = photo.Replace(path + "\\", string.Empty).Split('.');
                FileInfo fi = new FileInfo(photo);
                fi.MoveTo(path + "/image" + counter + "." + filename[1]);
                counter++;
            }

            counter = 1;
            foreach (int i in image)
            {
                string[] file = Directory.GetFiles(path).Where(img => img.Contains("image" + counter)).First().Split('.');
                new FileInfo(file[0] + "." + file[1]).MoveTo(path + "/" + i + "." + file[1]);
                counter++;
            }

            return RedirectToAction("Index", "Controlpanel");
        }

        // GET: Dresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            if (dress == null)
            {
                return NotFound();
            }

            return View(dress);
        }

        // POST: Dresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dress = await _context.Dress.SingleOrDefaultAsync(m => m.ID == id);
            _context.Dress.Remove(dress);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DressExists(int id)
        {
            return _context.Dress.Any(e => e.ID == id);
        }
    }
}
