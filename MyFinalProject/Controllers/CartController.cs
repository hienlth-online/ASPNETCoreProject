using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;
using HienlthOnline.Helpers;
using MyFinalProject.Models;

namespace MyFinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly MyStore2020Context _context;

        public CartController(MyStore2020Context context)
        {
            _context = context;
        }

        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        public IActionResult Index()
        {
            return View(Carts);
        }

        public IActionResult AddToCart(int id, int SoLuong, string type = "Normal")
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.MaHh == id);

            if(item == null)//chưa có
            {
                var hangHoa = _context.HangHoa.SingleOrDefault(p => p.MaHh == id);
                item = new CartItem
                {
                    MaHh = id,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia.Value,
                    SoLuong = SoLuong,
                    Hinh = hangHoa.Hinh
                };
                myCart.Add(item);
            }
            else
            {
                item.SoLuong += SoLuong;
            }
            HttpContext.Session.Set("GioHang", myCart);

            if(type == "ajax")
            {
                return Json(new { 
                    SoLuong = Carts.Sum(c => c.SoLuong)
                });
            }
            return RedirectToAction("Index");
        }
    }
}