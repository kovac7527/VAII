using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;

namespace VAII.Controllers.Admin
{
    public class CMSServisDevicesController : Controller
    {
        private readonly DataContext _context;

        public CMSServisDevicesController(DataContext context)
        {
            _context = context;
        }

        // GET: CMSServisDevices
        public async Task<IActionResult> Index(int? id)
        {

            ViewBag.SeriesId = id;
            ViewBag.SeriesName = _context.BrandSeries.FirstOrDefault(b => b.Id == id)?.Name;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Model,Name,ImagePath,Type")] ServisDevice servisDevice)
        {
            if (id != servisDevice.Id)
            {
                return NotFound();
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
                return RedirectToAction(nameof(Index));
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
    }
}
