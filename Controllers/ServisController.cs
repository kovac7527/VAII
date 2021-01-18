using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
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


            List<ServisAction> actionList = new List<ServisAction>();

            
            ServisModel model = new ServisModel();
            
            var brands = await _context.DeviceBrands.ToListAsync();
            List<SelectListItem> liListOfBrands = new List<SelectListItem>();
            liListOfBrands.Add(new SelectListItem("Vyberte značku" , "0"));

            foreach (var brand in brands)
            {
                liListOfBrands.Add(new SelectListItem(brand.Name, brand.Id.ToString()));
            }

            ViewBag.Brands = liListOfBrands;
            ViewBag.BrandId = 0;
            ViewBag.BrandSeries = 0;
            var actualSeries =  _context.BrandSeries.Include(d=>d.ServisDevices).FirstOrDefault(s => s.Id == id);
            if (actualSeries != null)
            {
                ViewBag.BrandId = actualSeries.DeviceBrandId;
                ViewBag.BrandSeries = actualSeries.Id;
                
            }
            
            return View(actualSeries);
        }


        public async Task<IActionResult> Details(int? id)
        {
            

            var device = _context.ServisDevices.Include(d => d.ServisActions).FirstOrDefault(d => d.Id == id);
            
            List<SelectListItem> liListOfBrands = new List<SelectListItem>();
           
            return View(device);
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

        public async void GenerateServisActions()
        {
            List<ServisAction> actionList = new List<ServisAction>();

            actionList.Add(new ServisAction()
            {
                Name = "Výmena LCD",
                HoursTofix = 1,
                DifficultyLevel = 2,
            });

            actionList.Add(new ServisAction()
            {
                Name = "Výmena Batérie",
                HoursTofix = 1,
                DifficultyLevel = 1,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Výmena Zadného krytu",
                HoursTofix = 1,
                DifficultyLevel = 1,

            });


            actionList.Add(new ServisAction()
            {
                Name = "Obliatie tekutinou",
                HoursTofix = 5,
                DifficultyLevel = 4,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Oprava nefunkčného mikrofónu",
                HoursTofix = 5,
                DifficultyLevel = 4,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Oprava nefunkčného reproduktoru",
                HoursTofix = 2,
                DifficultyLevel = 3,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Oprava nefunkčnej kamery",
                HoursTofix = 1,
                DifficultyLevel = 3,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Oprava nabíjania",
                HoursTofix = 4,
                DifficultyLevel = 5,

            });

            actionList.Add(new ServisAction()
            {
                Name = "Oprava matičnej dosky",
                HoursTofix = 99,
                DifficultyLevel = 5,

            });


            var devices = _context.ServisDevices.ToList();

            foreach (var device in devices)
            {
                foreach (var action in actionList)
                {
                    action.Id = 0;
                    action.ServisDeviceId = device.Id;
                }

                _context.ServisActions.AddRange(actionList);
                await _context.SaveChangesAsync();

            }
            
        }
    }
}
