using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaterCool.Models;
using WaterCool.Data;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WaterCool.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Self()
        {
            int id =Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            return RedirectToAction("Info","Home", new{ id = id } );
        }
        [Authorize]
        public IActionResult Info(int id)
        {
            User user = fakerDB.Users.SingleOrDefault(x => x.id == id);
            if(user != null)
            {
                var newlist =  fakerDB.Infos.Join(fakerDB.Users, Info => Info.userId, User => User.id, (Info, User) => new { infoid = Info.id, uid = User.id, username = User.Username, sex = Info.sex, city = Info.city, birthday = Info.birth, job = Info.job, introduce = Info.introduce, photoAddress = Info.photoAddress });
                var result = JsonConvert.SerializeObject(newlist.FirstOrDefault(x => x.uid == id));
                bool IsSelf = true;
                int uid = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
                ViewBag.uid = uid;
                if(id != uid)
                {
                    IsSelf = false;
                    bool IsFriend = true;
                    Friendship relation = fakerDB.Friends.FirstOrDefault( x => x.user_id == uid && x.friend_id == id );
                    if(relation == null)
                    {
                        IsFriend = false;
                    }
                    ViewBag.isFriend = IsFriend;
                }
                ViewBag.isself = IsSelf;
                if(result == "null")
                {
                    ViewBag.uid = user.id;
                    ViewBag.username = user.Username;
                    return View();
                }
                else
                {
                    var info = JsonConvert.DeserializeObject(result, typeof(InfoViewModel));
                    return View(info);
                }
            }
            else
            {
                ViewBag.errmsg = "用戶不存在";
                return View();
            }
            
            
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            var newlist =  fakerDB.Infos.Join(fakerDB.Users, Info => Info.userId, User => User.id, (Info, User) => new { infoid = Info.id, uid = User.id, username = User.Username, sex = Info.sex, city = Info.city, birthday = Info.birth, job = Info.job, introduce = Info.introduce, photoAddress = Info.photoAddress });
            var result = JsonConvert.SerializeObject(newlist.FirstOrDefault(x => x.uid == id));
            var info = JsonConvert.DeserializeObject(result, typeof(InfoViewModel));
            if(info != null)
            {
                return View(info);
            }else
            {
                string unmae = fakerDB.Users.SingleOrDefault(x => x.id == id).Username;
                var nullInfo = new InfoViewModel { uid = id, username = unmae};
                return View(nullInfo);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int uid, string sex, string birthday, string job, string introduce, string city, IFormFile pic)
        {
            string p ="";
            Info info = fakerDB.Infos.SingleOrDefault(x => x.userId == uid);
            /*if (pic != null || pic.Length != 0)
            {
                var path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\images\photo", 
                    pic.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await pic.CopyToAsync(stream);
                }
                p = "/images/photo/"+pic.FileName;
            }*/
            
            if(info == null)
            {
                fakerDB.Infos.Add( new Info
                { 
                    id = fakerDB.Infos.Max(x => x.id),
                    userId = uid,
                    sex = sex,
                    city = city,
                    birth = birthday,
                    job = job,
                    introduce = introduce,
                    photoAddress = p
                });
            }
            else
            {
                info.sex = sex;
                info.birth = birthday;
                info.job = job;
                info.introduce = introduce;
                info.city = city;
                info.photoAddress = p == "" ? info.photoAddress : p;
            }


            return RedirectToAction("Self");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [Authorize]
        public IActionResult Chat(string id)
        {
            ViewData["id"]=id;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
