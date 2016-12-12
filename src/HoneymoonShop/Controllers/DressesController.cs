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
using System.Text.RegularExpressions;

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
            var applicationDbContext = _context.Dress.Include(d => d.Color).Include(d => d.Manu).Include(d => d.Neckline).Include(d => d.Silhouette).Include(d => d.Style);
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
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "Name");
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "Name");
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "Name");
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "Name");
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "Name");
            return View();
        }

        // POST: Dresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ColorID,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dress);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "Name", dress.StyleID);
            return View(dress);
        }

        // GET: Dresses/AddImage
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
                string ss = s.Replace(path + "\\", string.Empty);
                if (ss.StartsWith(startw))
                {
                    files.Add(ss);
                }
            }
            ViewData["Images"] = files;
            return View();
        }

        // POST: Dresses/AddImage
        // Saves images in the images/dress folder, can only upload up to 3 images
        // TODO: return usefull message when trying to upload more than 3 images or when uploaded file is not accepted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(int? id, ICollection<IFormFile> files)
        {
            var uploads = Path.Combine(_env.WebRootPath, "images/dress");
            var manu = _context.Manu.Where(m => m.ID == _context.Dress.Where(d => d.ID == id).First().ManuID).First().Name.Replace(" ", string.Empty);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string[] s = file.FileName.Split('.');
                    string[] accepted = { "jpg", "jpeg", "png", "webp", "bmp"};
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

        // GET: Dresses/AddFeature/5
        // Add features to a specific dress (maybe replace function to DressFeaturesController?)
        public async Task<IActionResult> AddFeature(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dress = await _context.Dress.SingleOrDefaultAsync(d => d.ID == id);
            if (dress == null)
            {
                return NotFound();
            }

            List<Feature> CurrentFeatures = new List<Feature>();
            List<Feature> AvailableFeatures = new List<Feature>();
            foreach(DressFeature df in _context.DressFeature)
            {
                if(df.DressID == id)
                {
                    CurrentFeatures.Add(_context.Feature.Where(f => f.ID == df.FeatureID).First());
                }
            }
            foreach(Feature f in _context.Feature)
            {
                if(CurrentFeatures.Find(cf => cf.ID == f.ID) == null)
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

            if(ModelState.IsValid)
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
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "Name", dress.StyleID);
            return View(dress);
        }

        // POST: Dresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ColorID,ManuID,NecklineID,Price,SilhouetteID,StyleID")] Dress dress)
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
            ViewData["ColorID"] = new SelectList(_context.Set<Color>(), "ID", "Name", dress.ColorID);
            ViewData["ManuID"] = new SelectList(_context.Set<Manu>(), "ID", "Name", dress.ManuID);
            ViewData["NecklineID"] = new SelectList(_context.Set<Neckline>(), "ID", "Name", dress.NecklineID);
            ViewData["SilhouetteID"] = new SelectList(_context.Set<Silhouette>(), "ID", "Name", dress.SilhouetteID);
            ViewData["StyleID"] = new SelectList(_context.Set<Style>(), "ID", "Name", dress.StyleID);
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
