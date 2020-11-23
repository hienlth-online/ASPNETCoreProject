using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;
using MyFinalProject.ViewModels;

namespace MyFinalProject.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly MyStore2020Context _context;

        public HangHoaController(MyStore2020Context context)
        {
            _context = context;
        }

        private int SO_PHAN_TU_MOI_TRANG = 6;
        public IActionResult Index(int page = 1)
        {
            var dsHangHoa = _context.HangHoa
                .Skip((page - 1) * SO_PHAN_TU_MOI_TRANG)
                .Take(SO_PHAN_TU_MOI_TRANG)
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

        public IActionResult TimKiem()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TimAjax(string Keyword)
        {
            var dsHangHoa = _context.HangHoa.AsQueryable();

            if (!string.IsNullOrEmpty(Keyword))
            {
                dsHangHoa = dsHangHoa.Where(hh => hh.TenHh.Contains(Keyword));
            }

            var data = dsHangHoa.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                GiaBan = p.DonGia.Value,
                Hinh = p.Hinh,
                Loai = p.MaLoaiNavigation.TenLoai,
                NhaCungCap = p.MaNccNavigation.TenCongTy
            });

            return PartialView(data);
        }
    }
}