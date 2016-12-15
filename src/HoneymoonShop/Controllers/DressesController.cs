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

namespace HoneymoonShop.Controllers
{
    public class DressesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _env;

        public DressesController(ApplicationDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Dresses
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dress.Include(d => d.Category).Include(d => d.Manu).Include(d => d.Neckline).Include(d => d.Silhouette).Include(d => d.Style);
            return View(await applicationDbContext.ToListAsync());
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
                string startw = dress.ID.ToString();
                string filename = s.Replace(path + "\\", string.Empty);
                if (filename.StartsWith(startw))
                {
                    images.Add($"{id}/" + filename);
                }
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
            foreach (Dress d in _context.Dress)
            {
                if (d.ManuID == dress.ManuID) recDress.Add(d);
                if (recDress.Count <= 5) break;
            }
            if (recDress.Count <= 5)
            {
                foreach (Dress d in _context.Dress)
                {
                    if (d.StyleID == dress.StyleID) recDress.Add(d);
                    if (recDress.Count <= 5) break;
                }
            }

            //var recImg = new Dictionary<int, string>();
            //foreach (Dress d in recDress)
            //{
            //    path = Path.Combine(_env.WebRootPath, $"images/dress/{d.ID}");
            //    di = new DirectoryInfo(path);
            //    foreach (string file in Directory.GetFiles(path))
            //    {
            //        string filename = file.Replace(path + "\\", string.Empty);
            //        if (filename.StartsWith("1"))
            //        {
            //            recImg.Add(d.ID, $"{d.ID}/" + filename);
            //            break;
            //        }
            //    }
            //}

            ViewData["RecDress"] = recDress;
            //ViewData["RecImages"] = recImg;
            ViewData["Features"] = features;
            ViewData["Colors"] = colors;
            ViewData["Images"] = images;

            return View(dress);
        }

        // GET: Dresses/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID");
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID");
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "ID");
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "ID");
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID");
            return View();
        }

        // POST: Dresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,Description,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", dress.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", dress.StyleID);
            return View(dress);
        }

        // GET: Dresses/Overview
        public IActionResult Overview()
        {
            ViewData["CategoryID"] = _context.Category.ToList();
            ViewData["ColorID"] = _context.Color.ToList();
            ViewData["ManuID"] = _context.Manu.ToList();
            ViewData["NecklineID"] = _context.Neckline.ToList();
            ViewData["SilhouetteID"] = _context.Silhouette.ToList();
            ViewData["StyleID"] = _context.Style.ToList();

            List<Dress> availableDress = _context.Dress.Include(d => d.Manu).ToList();
            var img = new Dictionary<int, string>();
            foreach (Dress d in availableDress)
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
            ViewData["Dress"] = availableDress;
            return View();
        }

        [HttpGet]
        public IActionResult OverviewFiltered(string[] manu, string[] style, int pricemin, int pricemax, string[] neckline, string[] silhouette, string[] color)
        {
            List<Dress> availableDress = _context.Dress
                .Include(d => d.Manu)
                .Where(d => d.Price >= pricemin && d.Price <= pricemax)
                .ToList();
            List<Dress> toRemove = new List<Dress>();
            foreach (Dress d in availableDress)
            {
                bool bmanu = false;
                bool bstyle = false;
                bool bneck = false;
                bool bsil = false;
                bool bcolor = false;
                if (manu[0].Equals("all") && manu.Length == 1) bmanu = true;
                else
                {
                    for (int i = 1; i < manu.Length; i++)
                    {
                        if (int.Parse(manu[i]) == d.ManuID) bmanu = true;
                    }
                }

                if (style[0].Equals("all") && style.Length == 1) bstyle = true;
                else
                {
                    for (int i = 1; i < style.Length; i++)
                    {
                        if (int.Parse(style[i]) == d.StyleID) bstyle = true;
                    }
                }

                if (neckline[0].Equals("all") && neckline.Length == 1) bneck = true;
                else
                {
                    for (int i = 1; i < neckline.Length; i++)
                    {
                        if (int.Parse(neckline[i]) == d.NecklineID) bneck = true;
                    }
                }

                if (silhouette[0].Equals("all") && silhouette.Length == 1) bsil = true;
                else
                {
                    for (int i = 1; i < silhouette.Length; i++)
                    {
                        if (int.Parse(silhouette[i]) == d.SilhouetteID) bsil = true;
                    }
                }

                if (color[0].Equals("all") && color.Length == 1) bcolor = true;
                else
                {
                    List<DressColor> temp = _context.DressColor.Where(dc => dc.DressID == d.ID).ToList();
                    List<int> colors = new List<int>();
                    foreach (DressColor dc in temp)
                    {
                        colors.Add(dc.ColorID);
                    }

                    for (int i = 1; i < color.Length; i++)
                    {
                        if (colors.Contains(int.Parse(color[i]))) bcolor = true;
                    }
                }

                if (bmanu == false || bstyle == false || bneck == false || bsil == false || bcolor == false)
                {
                    toRemove.Add(d);
                }
            }
            foreach (Dress d in toRemove)
            {
                availableDress.Remove(d);
            }

            var img = new Dictionary<int, string>();
            foreach (Dress d in availableDress)
            {
                string path = Path.Combine(_env.WebRootPath, $"images/dress/{d.ID}");
                DirectoryInfo di = new DirectoryInfo(path);
                foreach (string s in Directory.GetFiles(path))
                {
                    string filename = s.Replace(path + "\\", string.Empty);
                    if(filename.StartsWith("1")) img.Add(d.ID, $"{d.ID}/" + filename);
                }
            }

            ViewData["Images"] = img;
            ViewData["Dress"] = availableDress;
            ViewData["CategoryID"] = _context.Category.ToList();
            ViewData["ColorID"] = _context.Color.ToList();
            ViewData["ManuID"] = _context.Manu.ToList();
            ViewData["NecklineID"] = _context.Neckline.ToList();
            ViewData["SilhouetteID"] = _context.Silhouette.ToList();
            ViewData["StyleID"] = _context.Style.ToList();
            return View("Overview");
        }

        // GET: Dresses/AddImage/5
        public IActionResult AddImage(int? id)
        {
            var dress = _context.Dress.Where(d => d.ID == id).First();
            try
            {
                string path = Path.Combine(_env.WebRootPath, $"images/dress/{id}");
                DirectoryInfo di = new DirectoryInfo(path);
                List<string> files = new List<string>();
                foreach (string s in Directory.GetFiles(path))
                {
                    string filename = s.Replace(path + "\\", string.Empty);
                    files.Add($"{id}/" + filename);
                }
                ViewData["Images"] = files;
            } catch
            {
                ViewData["Images"] = new List<string>();
            }

            return View();
        }

        // POST: Dresses/AddImage
        // Saves images in the images/dress folder, can only upload up to 4 images
        // TODO: return usefull message when trying to upload more than 4 images or when uploaded file is not accepted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int? id, ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_env.WebRootPath, $"images/dress/{id}");
            Directory.CreateDirectory(uploads);
            string[] accepted = { "jpg", "jpeg", "png", "webp", "bmp" };

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string[] s = file.FileName.Split('.');
                    foreach (string type in accepted)
                    {
                        if (s[1] == type)
                        {
                            try
                            {
                                var FileStream = new FileStream(Path.Combine(uploads, "1." + s[1]), FileMode.Create);
                                await file.CopyToAsync(FileStream);
                            }
                            catch
                            {
                                try
                                {
                                    var FileStream = new FileStream(Path.Combine(uploads, "2." + s[1]), FileMode.Create);
                                    await file.CopyToAsync(FileStream);
                                }
                                catch
                                {
                                    try
                                    {
                                        var FileStream = new FileStream(Path.Combine(uploads, "3." + s[1]), FileMode.Create);
                                        await file.CopyToAsync(FileStream);
                                    }
                                    catch
                                    {
                                        return NotFound();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return RedirectToAction("AddImage", id);
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

            if(ModelState.IsValid)
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

        // GET: Dresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", dress.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", dress.StyleID);
            return View(dress);
        }

        // POST: Dresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,Description,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (id != dress.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DressExists(dress.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "ID", dress.CategoryID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "ID", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "ID", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "ID", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "ID", dress.StyleID);
            return View(dress);
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
