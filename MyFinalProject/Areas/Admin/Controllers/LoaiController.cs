using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;

namespace MyFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoaiController : Controller
    {
        private readonly MyStore2020Context _context;

        public LoaiController(MyStore2020Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Loai);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Loai loai, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                if (Hinh != null)
                {
                    var urlFull = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", "Loai", Hinh.FileName);
                    using (var file = new FileStream(urlFull, FileMode.Create))
                    {
                        await Hinh.CopyToAsync(file);
                    }

                    loai.Hinh = Hinh.FileName;
                }

                _context.Add(loai);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var loai = _context.Loai.SingleOrDefault(lo => lo.MaLoai == id);
            if(loai == null)
            {
                return RedirectToAction("Index");
            }

            return View(loai);
        }

        [HttpPost]
        public IActionResult Edit(Loai loai, IFormFile HinhUpload)
        {
            if (ModelState.IsValid)
            {
                if (HinhUpload != null)
                {
                    var urlFull = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", "Loai", HinhUpload.FileName);
                    using (var file = new FileStream(urlFull, FileMode.Create))
                    {
                        HinhUpload.CopyTo(file);
                    }

                    loai.Hinh = HinhUpload.FileName;
                }

                _context.Update(loai);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }
    }
}