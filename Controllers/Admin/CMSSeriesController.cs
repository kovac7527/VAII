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
    public class CMSSeriesController : Controller
    {
        private readonly DataContext _context;

        public CMSSeriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Series
        public async Task<IActionResult> Index(int? id)
        {
            ViewBag.BrandID = id;
            ViewBag.BrandName = _context.DeviceBrands.FirstOrDefault(b => b.Id == id)?.Name;
            return View(await _context.BrandSeries.Where(bs=> bs.DeviceBrandId == id ).ToListAsync());
        }

        // GET: Series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.BrandSeries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        // GET: Series/Create
        public IActionResult Create(int? id)
        {
            if (_context.DeviceBrands.FirstOrDefault(b => b.Id == id) == null)
            {
                return RedirectToAction("Index", "CMSBrands");
            }

            ViewBag.BrandID = id;
            ViewBag.BrandName = _context.DeviceBrands.FirstOrDefault(b => b.Id == id)?.Name;
            return View();
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,DeviceBrandId")] Series series, int? id)
        {
           DeviceBrand brand = _context.DeviceBrands.FirstOrDefault(b => b.Id == id);
           brand.SeriesList = new List<Series>();
           
           if (brand != null)
           {
               brand.SeriesList.Add(series);
               
               await _context.SaveChangesAsync();
               return RedirectToAction("Index", new{ id = id });
           }

           
            
           
           return View(series);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.BrandSeries.FindAsync(id);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DeviceBrandId,Name,Description")] Series series)
        {
            if (id != series.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(series);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriesExists(series.Id))
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
            return View(series);
        }

        // GET: Series/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var series = await _context.BrandSeries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (series == null)
            {
                return NotFound();
            }

            return View(series);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var series = await _context.BrandSeries.FindAsync(id);
            _context.BrandSeries.Remove(series);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriesExists(int id)
        {
            return _context.BrandSeries.Any(e => e.Id == id);
        }
    }
}
