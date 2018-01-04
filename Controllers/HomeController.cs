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

namespace WaterCool.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Info(int id)
        {
            var newlist =  fakerDB.Infos.Join(fakerDB.Users, Info => Info.userId, User => User.id, (Info, User) => new { infoid = Info.id, uid = User.id, username = User.Username, sex = Info.sex, city = Info.city, birthday = Info.birth, job = Info.job, introduce = Info.introduce, photoAddress = Info.photoAddress });
            var result = JsonConvert.SerializeObject(newlist.FirstOrDefault(x => x.uid == id));
            var info = JsonConvert.DeserializeObject(result, typeof(InfoViewModel));
            return View(info);
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
