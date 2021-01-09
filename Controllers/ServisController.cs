using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccesLib.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VAII.Models;

namespace VAII.Controllers
{
    public class ServisController : Controller
    {
        private readonly DataContext _context;
        public ServisController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            ServisModel model = new ServisModel();
            
            var brands = await _context.DeviceBrands.ToListAsync();
            List<SelectListItem> liListOfBrands = new List<SelectListItem>();
            liListOfBrands.Add(new SelectListItem("Vyberte značku" , "0"));

            foreach (var brand in brands)
            {
                liListOfBrands.Add(new SelectListItem(brand.Name, brand.Id.ToString()));
            }

            ViewBag.Brands = liListOfBrands;
            var AllBrands = await _context.DeviceBrands.Include(b=>b.SeriesList).ThenInclude(s=>s.ServisDevices).ToListAsync();
            return View(model);
        }

        public JsonResult GetSeries(int id)
        { 
            var series = _context.BrandSeries.Where(b => b.DeviceBrandId == id).ToList();
            List<SelectListItem> liListOfSeries = new List<SelectListItem>();
            liListOfSeries.Add(new SelectListItem("Vyberte sériu", "0"));

            if (series != null)
            {
                foreach (var item in series)
                {
                    liListOfSeries.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });
                }
            }

            return Json(liListOfSeries);
        }

        public JsonResult GetDevices(int id)
        {
            var devices = _context.ServisDevices.Where(s => s.SeriesId == id).ToList();
            return Json(devices);
            
        }
    }
}
