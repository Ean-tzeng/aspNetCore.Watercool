using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaterCool.Models;
using WaterCool.Data;
using System.Security.Claims;

namespace WaterCool.Controllers
{
    public class PostController : Controller
    {
        [Authorize()]
        public IActionResult Post()
        {
            return View();
        }
        [Authorize()]
        [HttpPost]
        public IActionResult Post(  string content )
        {
            PostsModel post = new PostsModel()
            {
                postId = fakerDB.Posts.Count > 0 ? fakerDB.Posts.Max(x => x.postId)+1 : 0,
                userId = Int32.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value),
                context = content,
                update = DateTime.Now.ToString()
            };
            fakerDB.Posts.Add(post);
            return new JsonResult(post);
        }
        [Authorize()]
        [HttpPost]
        public IActionResult GetPost(int count)
        {
            //int idx = Int32.Parse( HttpContext.User.FindFirst(ClaimTypes.Sid).Value );
            int idx = 1;
            List<Friendship> Friends = fakerDB.Friends.FindAll(x => x.friend_id == idx);
            List<PostsModel> posts = fakerDB.Posts.FindAll(x => x.userId == idx);
            foreach( Friendship F in Friends)
            {
                List<PostsModel> tmp =  fakerDB.Posts.FindAll(x => x.userId == F.user_id);
                foreach( PostsModel p in tmp)
                {
                    posts.Add(p);
                }
            }
            var result = fakerDB.Users.Join(posts,  User => User.id,PostsModel => PostsModel.userId,  ( User, PostsModel) => new { postId = PostsModel.postId ,userId = User.id, Author = User.Username, context = PostsModel.context, update = PostsModel.update } );
            return new ObjectResult(result.OrderByDescending(x => x.update).Take(count));
        }
    }
}