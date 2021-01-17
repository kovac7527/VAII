using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace VAII.Controllers.Admin
{
    public class CMSServisDevicesController : Controller 
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CMSServisDevicesController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
            _context = context;
        }

        // GET: CMSServisDevices
        public async Task<IActionResult> Index(int? id)
        {

            ViewBag.SeriesId = id;
            var series = _context.BrandSeries.FirstOrDefault(b => b.Id == id);
            if (series != null)
            {
                ViewBag.SeriesName = series.Name;
                ViewBag.BrandId = series.DeviceBrandId;
                var brand = _context.DeviceBrands.FirstOrDefault(b => b.Id == series.DeviceBrandId);
                ViewBag.BrandName = brand.Name;
                ViewBag.BrandLogo = brand.ImageLogoPath;
            }

            return View(await _context.ServisDevices.Where(s=>s.SeriesId == id).ToListAsync());
            
        }

        // GET: CMSServisDevices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisDevice = await _context.ServisDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servisDevice == null)
            {
                return NotFound();
            }

            return View(servisDevice);
        }

        // GET: CMSServisDevices/Create
        public IActionResult Create(int? id)
        {
            if (_context.BrandSeries.FirstOrDefault(b => b.Id == id) == null)
            {
                return RedirectToAction("Index", "CMSBrands");
            }

            ViewBag.SeriesId = id;
            ViewBag.SeriesName = _context.BrandSeries.FirstOrDefault(b => b.Id == id)?.Name;
            return View();

        }

        // POST: CMSServisDevices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Model,Name,ImagePath")] ServisDevice servisDevice, int? id)
        {
           
            Series series = _context.BrandSeries.FirstOrDefault(s => s.Id == id);
            

            if (series != null)
            {
                series.ServisDevices = new List<ServisDevice>();
                series.ServisDevices.Add(servisDevice);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { id = id });
            }




            return View(servisDevice);
        }

        // GET: CMSServisDevices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisDevice = await _context.ServisDevices.FindAsync(id);
            if (servisDevice == null)
            {
                return NotFound();
            }
            return View(servisDevice);
        }

        // POST: CMSServisDevices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeriesId,Model,Name,ImagePath,Type")] ServisDevice servisDevice, string imguploader)
        {
            string newImage= HttpContext.Request.Form["imguploader"];

            if (id != servisDevice.Id)
            {
                return NotFound();
            }

            if (!String.IsNullOrEmpty(newImage))
            {
                servisDevice.ImagePath = newImage;


            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servisDevice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServisDeviceExists(servisDevice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                
            }
            return View(servisDevice);
        }

        // GET: CMSServisDevices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servisDevice = await _context.ServisDevices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servisDevice == null)
            {
                return NotFound();
            }

            return View(servisDevice);
        }

        // POST: CMSServisDevices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servisDevice = await _context.ServisDevices.FindAsync(id);
            _context.ServisDevices.Remove(servisDevice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServisDeviceExists(int id)
        {
            return _context.ServisDevices.Any(e => e.Id == id);
        }



        [HttpPost]
        public async Task<IActionResult> InsertImage(IFormFile imgFile)
        {

            if (imgFile != null && imgFile.Length != 0)
            {
                
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "UploadedImgs","ServisDevices",
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
