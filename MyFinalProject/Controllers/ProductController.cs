using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;
using MyFinalProject.ViewModels;

namespace MyFinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyStore2020Context _context;

        public ProductController(MyStore2020Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var dsHangHoa = _context.HangHoa
                .Select(p => new HangHoaVM
                {
                    MaHh = p.MaHh,
                    TenHh = p.TenHh,
                    GiaBan = p.DonGia.Value,
                    Hinh = p.Hinh,
                    Loai = p.MaLoaiNavigation.TenLoai,
                    NhaCungCap = p.MaNccNavigation.TenCongTy
                });

            return View(dsHangHoa);
        }

        public IActionResult Detail(int id)
        {
            var item = _context.HangHoa.SingleOrDefault(hh => hh.MaHh == id);
            if(item != null)
            {
                return View(item);
            }

            return RedirectToAction("Index");
        }
    }
}