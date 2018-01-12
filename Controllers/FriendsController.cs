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
            List<Friendship> newList = fakerDB.Friends.Join(fakerDB.Infos, Friendship => Friendship.friend_id, Info => Info.userId , (Friendship, info) => new Friendship{ user_id = Friendship.user_id, friend_id = Friendship.friend_id, fri_name = Friendship.fri_name, photo = info.photoAddress }).Cast<Friendship>().ToList();
            List<Friendship> FriendsList = newList.FindAll(x => x.user_id == u_Id);
            return View(FriendsList);
            
        }
        [Authorize]
        [HttpPost]
        public IActionResult getMessage(string from, int num)
        {
            int come = Int32.Parse(from);
            int go = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            List<MessageModel> resultA = fakerDB.Messages.FindAll( x => x.FromId == come && x.ToId == go);
            List<MessageModel> resultB = fakerDB.Messages.FindAll(x => x.FromId == go && x.ToId == come);
            var  resultC =(resultA.Union(resultB)).OrderBy(x => x.date);
            int count = resultC.Count()-num;
            if(count < 0)
            {
                count = 0;
            }
            var result = resultC.Skip(count).Take(num);
            return new ObjectResult(result);
        }
        [Authorize]
        [HttpPost]
        public IActionResult MakeFriend( int id )
        {
            int uid =Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value);
            int fid = id;
            Friendship relation = fakerDB.Friends.FirstOrDefault(x => x.user_id == uid && x.friend_id == fid);
            if(relation != null)
            {
                return Json(new { status="error",message="已是好友關係"});
            }
            else
            {
                fakerDB.Friends.Add( new Friendship{ user_id = uid, friend_id = fid, fri_name = fakerDB.Users.FirstOrDefault(x => x.id == fid).Username });
                fakerDB.Friends.Add( new Friendship{ user_id = fid, friend_id = uid, fri_name = fakerDB.Users.FirstOrDefault(x => x.id == uid).Username });
                return Json(new { status="success",message="你們已成為好友"});
            }
            
        }
        [Authorize]
        [HttpPost]
        public IActionResult SearchFriends( string Username)
        {
            List<User> newList = fakerDB.Users.Join(fakerDB.Infos, User => User.id, Info => Info.userId , (User, info) => new User{ id = User.id, Username = User.Username, photo = info.photoAddress }).Cast<User>().ToList();
            List<User> UserList = newList.FindAll(x => x.Username == Username);
            if(UserList.Count <=0)
            {
                ViewBag.msg = "查無此人!";
                return View();
            }
            return View(UserList);
        }

    }
}