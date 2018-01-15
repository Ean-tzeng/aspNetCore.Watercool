using System;
using System.Collections.Generic;

namespace WaterCool.Models
{
    public class PostsModel
    {
        public int postId;
        public int userId;
        public string context;
        public String update;
        public String Likes;
        public List<CommendModel> commend;
    }
}