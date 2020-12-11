using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFinalProject.Entities;
using MyFinalProject.Helpers;
using MyFinalProject.Models;

namespace MyFinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyStore2020Context _context;
        private readonly AppSettings _appSettings;

        public UserController(MyStore2020Context db, IOptions<AppSettings> appSettings)
        {
            _context = db;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
            var user = _context.KhachHang.SingleOrDefault(kh => kh.MaKh == loginVM.Username && kh.MatKhau == loginVM.Password);

            if(user == null)
            {
                return Ok(new ApiMessage
                {
                    Success = false,
                    Message = "Sai thông tin đăng nhập"
                });
            }


            //Generate token
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.HoTen),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("id", user.MaKh),
                new Claim(ClaimTypes.Role, "KhachHang")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new ApiMessage
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Data = new
                {
                    Token = tokenHandler.WriteToken(token)
                }
            });
        }
    }
}