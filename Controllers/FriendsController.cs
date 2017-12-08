using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WaterCool.Models;
using WaterCool.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace WaterCool.Controllers
{
    public class FriendsController : Controller
    {
        [Authorize]
        public IActionResult List()
        {
            int u_Id = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value) ;
            List<Friendship> FriendsList = fakerDB.Friends.FindAll(x => x.user_id == u_Id);
            return View(FriendsList);
            
        }
        
    }
}