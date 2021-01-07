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
    public class CMSDeviceBrandsController : Controller
    {
        private readonly DataContext _context;

        public CMSDeviceBrandsController(DataContext context)
        {
            _context = context;
        }

        // GET: CMSDeviceBrands
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeviceBrands.ToListAsync());
        }

        // GET: CMSDeviceBrands/Details/5
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

        // GET: CMSDeviceBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CMSDeviceBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ImageLogoPath")] DeviceBrand deviceBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deviceBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deviceBrand);
        }

        // GET: CMSDeviceBrands/Edit/5
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

        // POST: CMSDeviceBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageLogoPath,Description")] DeviceBrand deviceBrand)
        {
            if (id != deviceBrand.Id)
            {
                return NotFound();
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
                return RedirectToAction(nameof(Index));
            }
            return View(deviceBrand);
        }

        // GET: CMSDeviceBrands/Delete/5
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

        // POST: CMSDeviceBrands/Delete/5
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


        private bool IsLoggedIn()
        {

            if (HttpContext.Session.TryGetValue("username", out var name))
            {
                TempData["user"] = System.Text.Encoding.Default.GetString(name);
                return true;
            }

            RedirectToAction("Index", "Admin");
            return false;

        }
    }
}
