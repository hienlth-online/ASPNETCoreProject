using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Entities;
using MyFinalProject.Models;
using MyFinalProject.Services;

namespace MyFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoaiController : ControllerBase
    {
        //private readonly MyStore2020Context _context;
        private readonly ILoaiService _loaiService;

        public LoaiController(ILoaiService loaiService)//MyStore2020Context db)
        {
            //_context = db;
            _loaiService = loaiService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //return this.Ok(_context.Loai.ToList());
            return this.Ok(_loaiService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //var loai = _context.Loai.SingleOrDefault(p => p.MaLoai == id);
            var loai = _loaiService.GetById(id);
            if(loai != null)
            {
                return Ok(loai);
            }
            return this.BadRequest();
        }

        [HttpPost]
        public IActionResult AddLoai(Loai loai)
        {
            try
            {
                //_context.Add(loai);
                //_context.SaveChanges();
                return Ok(new ApiMessage
                {
                    Success = true,
                    Message = "Thêm mới thành công"
                });
            }
            catch
            {
                //return StatusCode(500);
                return Ok(new ApiMessage
                {
                    Success = false,
                    Message = "Thêm mới thất bại"
                });
            }
        }
    }
}