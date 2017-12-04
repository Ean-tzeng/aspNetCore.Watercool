using System;
using System.Collections.Generic;
using WaterCool.Models;

namespace WaterCool.Data
{
    public class fakerDB
    {
        public static List<User> Users = new List<User>
        {
            new User{ id = 1, Username="test", password="test", role="Member" },
            new User{ id = 2, Username="admin", password="admin", role="admin" },
            new User{ id = 3, Username="friend1", password="friend1", role="Member"},
            new User{ id = 4, Username="friend2", password="friend2", role="Member"},
            new User{ id = 5, Username="friend3", password="friend3", role="Member"},
            
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
        
    }
}