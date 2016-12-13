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
            var applicationDbContext = _context.Dress.Include(d => d.Category).Include(d => d.Color).Include(d => d.Manu).Include(d => d.Neckline).Include(d => d.Silhouette).Include(d => d.Style);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dresses/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Dresses/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name");
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "Name");
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "Name");
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "Name");
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "Name");
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "Name");
            return View();
        }

        // POST: Dresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryID,ColorID,Description,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", dress.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "Name", dress.StyleID);
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
            ViewData["Dress"] = new List<Dress>();
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult ProcessFilter()
        //{
        //    var manu = HttpContext.Request.Form["manu"];
        //    if (manu.Count == 0) manu = new string[] { "all" };
        //    var style = HttpContext.Request.Form["style"];
        //    if (style.Count == 0) style = new string[] { "all" };
        //    var pricemin = int.Parse(HttpContext.Request.Form["pricemin"]);
        //    var pricemax = int.Parse(HttpContext.Request.Form["pricemax"]);
        //    var neckline = HttpContext.Request.Form["neckline"];
        //    if (neckline.Count == 0) neckline = new string[] { "all" };
        //    var silhouette = HttpContext.Request.Form["silhouette"];
        //    if (silhouette.Count == 0) silhouette = new string[] { "all" };
        //    var color = HttpContext.Request.Form["color"];
        //    if (color.Count == 0) color = new string[] { "all" };
        //    return RedirectToAction("Overview", new { manu = manu.ToArray(), style = style.ToArray(), pricemin = pricemin, pricemax = pricemax, neckline = neckline.ToArray(), silhouette = silhouette.ToArray(), color = color.ToArray() });
        //}

        [HttpGet]
        public IActionResult OverviewFiltered(string[] manu, string[] style, int pricemin, int pricemax, string[] neckline, string[] silhouette, string[] color)
        {
            List<Dress> availableDress = _context.Dress.Where(d => d.Price >= pricemin && d.Price <= pricemax).ToList();
            if (availableDress.Count > 0) foreach (Dress d in availableDress)
                {
                    bool bmanu = false;
                    bool bstyle = false;
                    bool bneck = false;
                    bool bsil = false;
                    bool bcolor = false;
                    if (manu[0].Equals("all") && manu.Length == 1) bmanu = true;
                    else
                    {
                        manu[0] = "-1";
                        foreach (string s in manu)
                        {
                            if (int.Parse(s) == d.ManuID) bmanu = true;
                        }
                    }

                    if (style[0].Equals("all") && style.Length == 1) bstyle = true;
                    else
                    {
                        style[0] = "-1";
                        foreach (string s in style)
                        {
                            if (int.Parse(s) == d.StyleID) bstyle = true;
                        }
                    }

                    if (neckline[0].Equals("all") && neckline.Length == 1) bneck = true;
                    else
                    {
                        neckline[0] = "-1";
                        foreach (string s in neckline)
                        {
                            if (int.Parse(s) == d.NecklineID) bneck = true;
                        }
                    }

                    if (silhouette[0].Equals("all") && silhouette.Length == 1) bsil = true;
                    else
                    {
                        silhouette[0] = "-1";
                        foreach (string s in silhouette)
                        {
                            if (int.Parse(s) == d.SilhouetteID) bsil = true;
                        }
                    }

                    if (color[0].Equals("all") && color.Length == 1) bcolor = true;
                    else
                    {
                        color[0] = "-1";
                        foreach (string s in color)
                        {
                            if (int.Parse(s) == d.ColorID) bcolor = true;
                        }
                    }

                    if (bmanu == false || bstyle == false || bneck == false || bsil == false || bcolor == false)
                    {
                        availableDress.Remove(d);
                    }
                }
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
            var manu = _context.Manu.Where(m => m.ID == dress.ManuID).First().Name.Replace(" ", string.Empty);

            string path = Path.Combine(_env.WebRootPath, "images/dress");
            DirectoryInfo di = new DirectoryInfo(path);
            List<string> files = new List<string>();
            foreach (string s in Directory.GetFiles(path))
            {
                string startw = dress.ID.ToString() + manu;
                string filename = s.Replace(path + "\\", string.Empty);
                if (filename.StartsWith(startw))
                {
                    files.Add(filename);
                }
            }
            ViewData["Images"] = files;

            return View();
        }

        // POST: Dresses/AddImage
        // Saves images in the images/dress folder, can only upload up to 4 images
        // TODO: return usefull message when trying to upload more than 4 images or when uploaded file is not accepted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int? id, ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images/dress");
            var manu = _context.Manu.Where(m => m.ID == _context.Dress.Where(d => d.ID == id).First().ManuID).First().Name.Replace(" ", string.Empty);
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
                                var FileStream = new FileStream(Path.Combine(uploads, id + manu + "1." + s[1]), FileMode.Create);
                                await file.CopyToAsync(FileStream);
                            }
                            catch
                            {
                                try
                                {
                                    var FileStream = new FileStream(Path.Combine(uploads, id + manu + "2." + s[1]), FileMode.Create);
                                    await file.CopyToAsync(FileStream);
                                }
                                catch
                                {
                                    try
                                    {
                                        var FileStream = new FileStream(Path.Combine(uploads, id + manu + "3." + s[1]), FileMode.Create);
                                        await file.CopyToAsync(FileStream);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            var FileStream = new FileStream(Path.Combine(uploads, id + manu + "4." + s[1]), FileMode.Create);
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
            }
            return RedirectToAction("AddImage", id);
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", dress.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "Name", dress.StyleID);
            return View(dress);
        }

        // POST: Dresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryID,ColorID,Description,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
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
            ViewData["CategoryID"] = new SelectList(_context.Category, "ID", "Name", dress.CategoryID);
            ViewData["ColorID"] = new SelectList(_context.Color, "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Manu, "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Neckline, "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Silhouette, "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Style, "ID", "Name", dress.StyleID);
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
