using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WaterCool.Models;
using WaterCool.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace WaterCool.Controllers
{
    public class AuthController :Controller
    {
         private IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }
         public IActionResult Login(string returnUrl)
         {
             TempData["returnUrl"] = returnUrl;
             return View();
             
         }
         [HttpPost]
         public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
         {
            User user = fakerDB.Users.SingleOrDefault(s => s.Username == model.Username && s.password == model.password);            
            if(user != null)
            {
                ClaimsIdentity Identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                Identity.AddClaim(new Claim(ClaimTypes.Sid, user.id.ToString()));
                Identity.AddClaim(new Claim(ClaimTypes.Name, user.Username) );
                Identity.AddClaim( new Claim(ClaimTypes.Role, user.role) );
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Identity));
                if (returnUrl == null)
                 {
                     returnUrl = TempData["returnUrl"]?.ToString();
                 }
                 if (returnUrl != null)
                 {
                     return Redirect(returnUrl);
                 }
                 else
                 {
                     return RedirectToAction(nameof(PostController.Post), "Post");
                 }
            }
            else
            {
                if(user == null)
                {
                    ViewData["noUser"] = "此帳號尚未註冊!";
                }else{
                    ViewData["noUser"] = "帳號密碼錯誤";
                }
                
                return View(model);
            }
         }
         public  IActionResult SignUp()
         {
              return View();
         }
         [HttpPost]
         public IActionResult SignUp(LoginViewModel model, string returnUrl = null)
         {
            User user = fakerDB.Users.SingleOrDefault(s => s.Username == model.Username && s.password == model.password);            
            if(user != null )
            {
                ViewData["noUser"]="帳號重複";
                return View(model);
            }
            else
            {
                int i = fakerDB.Users.Max(x => x.id)+1;
                fakerDB.Users.Add(new User{ id = i, Username=model.Username, password=model.password, role="Member", connectionId="" });
                fakerDB.Infos.Add(new Info { id = fakerDB.Infos.Max(x => x.id)+1, userId = i  });
                 return RedirectToAction(nameof(AuthController.Login), "Auth");
            }
            
         }
         public async Task<IActionResult> Logout()
         {
             await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
             return RedirectToAction(nameof(PostController.Post), "Post");
         }
        [EnableCors("CorsPolicy")]  
         public IActionResult APILogin( [FromBody] LoginViewModel login)
         {
            User user = fakerDB.Users.FirstOrDefault(x => x.Username == login.Username && x.password == login.password);
            if(user != null)
            {
                var tokenString = BuildToken(user);
                return Ok(new { token = tokenString });
            }
            return StatusCode(403);
         }
        private string BuildToken(User user)
        {

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Issuer"],
            _config["Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
