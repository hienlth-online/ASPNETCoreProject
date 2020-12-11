using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Services;

namespace MyFinalProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILoaiService _loaiService;

        public CategoryController(ILoaiService loaiService)
        {
            _loaiService = loaiService;
        }
        public IActionResult Index()
        {
            return View(_loaiService.GetAll());
        }
    }
}