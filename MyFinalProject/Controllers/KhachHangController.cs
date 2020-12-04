using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;
using MyFinalProject.Models;

namespace MyFinalProject.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        private readonly MyStore2020Context _context;

        public KhachHangController(MyStore2020Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.KhachHang.Select(p => new KhachHangVM
            {
                MaKh = p.MaKh, HoTen = p.HoTen,
                Email = p.Email, DiaChi = p.DiaChi
            });
            return View(data);
        }

        [AllowAnonymous, HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string ReturnUrl = null)
        {
            var khachHang = _context.KhachHang.SingleOrDefault(kh => kh.MaKh == loginVM.MaKh && kh.MatKhau == loginVM.MatKhau);
            if(khachHang == null)
            {
                ViewBag.Loi = "Sai thông tin đăng nhập";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, khachHang.HoTen),
                new Claim(ClaimTypes.Email, khachHang.Email),
                new Claim("MaKH", khachHang.MaKh),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "QuanTriHeThong"),
                new Claim(ClaimTypes.Role, "BanHang")
            };

            // create identity
            var userIdentity = new ClaimsIdentity(claims, "login");
            // create principal
            var principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);

            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Profile");
        }

        public IActionResult Profile()
        {
            return View();
        }

        //[Authorize(Roles ="KhachHang")]
        public IActionResult Promotion()
        {
            if(!User.IsInRole("KhachHang"))
            {
                return RedirectToAction("AccessDenied");
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}