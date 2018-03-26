using System;
using System.Collections.Generic;
using WaterCool.Models;

namespace WaterCool.Data
{
    public class fakerDB
    {
        public static List<User> Users = new List<User>
        {
            new User{ id = 1, Username="test", password="test", role="Member", connectionId="" },
            new User{ id = 2, Username="admin", password="admin", role="admin", connectionId="" },
            new User{ id = 3, Username="friend1", password="friend1", role="Member", connectionId=""},
            new User{ id = 4, Username="friend2", password="friend2", role="Member", connectionId=""},
            new User{ id = 5, Username="friend3", password="friend3", role="Member", connectionId=""},
            new User{ id = 6, Username="custom", password="custom", role="Member", connectionId="" },
            
            
        };
        public static List<Friendship> Friends = new List<Friendship>
        {
            new Friendship{ user_id = 1, friend_id = 2, fri_name ="admin"},
            new Friendship{ user_id = 2, friend_id = 1, fri_name ="test"},
            new Friendship{ user_id = 1, friend_id = 4, fri_name ="friend2"},
            new Friendship{ user_id = 4, friend_id = 1, fri_name ="test"},
            new Friendship{ user_id = 1, friend_id = 5, fri_name ="friend3"},
            new Friendship{ user_id = 5, friend_id = 1, fri_name ="test"},
        };
        public static List<MessageModel> Messages = new List<MessageModel>
        {
            
        };
        public static List<PostsModel> Posts = new List<PostsModel>
        {
            
        };
        public static List<Info> Infos = new List<Info>
        {
            new Info{ id= 1, userId = 1, sex = "男", city = "test的家", birth = "1990/03/13", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" },
            new Info{ id= 2, userId = 2, sex = "男", city = "admin的家", birth = "1990/03/13", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" },
            new Info{ id= 3, userId = 3, sex = "男", city = "friend1的家", birth = "1990/03/13", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" },
            new Info{ id= 4, userId = 4, sex = "男", city = "friend2的家", birth = "1990/03/13", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" },
            new Info{ id= 5, userId = 5, sex = "男", city = "friend3的家", birth = "1990/03/13", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" },
            new Info{ id= 6, userId = 6, sex = "男", city = "custom的家", birth = "1977/09/15", job = "eng", photoAddress = "http://pic.pimg.tw/aijdesign/1352948082-53184121.jpg" }
        };
        public static List<CommendModel> commends = new List<CommendModel>
        {
            
        };
    }
}