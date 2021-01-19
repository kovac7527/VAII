using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
using Microsoft.AspNetCore.Http;

namespace VAII.Controllers.Admin
{
    public class CMSBrandsController : Controller
    {
        private readonly DataContext _context;

        public CMSBrandsController(DataContext context)
        {
            _context = context;
            
        }

        // GET: CMSBrands
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeviceBrands.Include(b=>b.SeriesList).ToListAsync());
        }

        // GET: CMSBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceBrand = await _context.DeviceBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceBrand == null)
            {
                return NotFound();
            }

            return View(deviceBrand);
        }

        // GET: CMSBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMSBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImageLogoPath,Description")] DeviceBrand deviceBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceBrand);
        }

        // GET: CMSBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceBrand = await _context.DeviceBrands.FindAsync(id);
            if (deviceBrand == null)
            {
                return NotFound();
            }
            return View(deviceBrand);
        }

        // POST: CMSBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageLogoPath,Description")] DeviceBrand deviceBrand)
        {
            string newImage = HttpContext.Request.Form["imguploader"];

            

            if (id != deviceBrand.Id)
            {
                return NotFound();
            }
            
            if (!String.IsNullOrEmpty(newImage))
            {
                deviceBrand.ImageLogoPath = newImage;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceBrandExists(deviceBrand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(deviceBrand);
        }

        // GET: CMSBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deviceBrand = await _context.DeviceBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (deviceBrand == null)
            {
                return NotFound();
            }

            return View(deviceBrand);
        }

        // POST: CMSBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deviceBrand = await _context.DeviceBrands.FindAsync(id);
            _context.DeviceBrands.Remove(deviceBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceBrandExists(int id)
        {
            return _context.DeviceBrands.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> InsertImage(IFormFile imgFile)
        {

            if (imgFile != null && imgFile.Length != 0)
            {

                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "UploadedImgs", "BrandLogo",
                    imgFile.FileName);

                if (!System.IO.File.Exists(path))
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }

            return BadRequest();


        }

    }
}
