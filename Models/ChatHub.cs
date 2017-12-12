using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WaterCool.Data;
using WaterCool.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace WaterCool.Models
{
    public class MessageModel
    {
        public int msgid ;
        public int FromId;
        public string FromName;
        public int ToId;
        public string ToName;
        public string text;
        public String date;
        public bool isRead;
    }
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            int uid = Int32.Parse(Context.User.FindFirst(ClaimTypes.Sid).Value);
            User u = fakerDB.Users.Find(s => s.id == uid);
            u.connectionId = Context.ConnectionId;
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            int uid = Int32.Parse(Context.User.FindFirst(ClaimTypes.Sid).Value);
            User u = fakerDB.Users.Find(s => s.id == uid);
            u.connectionId = "";

            return base.OnDisconnectedAsync(exception);
        }
        public async  Task Send(string id, string message)
        {
            MessageModel m = new MessageModel();
            m.FromId = Int32.Parse(Context.User.FindFirst(ClaimTypes.Sid).Value);
            m.FromName= Context.User.FindFirst(ClaimTypes.Name).Value;
            m.ToId = Int32.Parse(id);
            m.text = message;
            User u = fakerDB.Users.Find(s => s.id == m.ToId);
            m.ToName =u.Username;
            m.date = DateTime.Now.ToString();
            fakerDB.Messages.Add(m);
            if(fakerDB.Messages.Count > 20)
            {
                fakerDB.Messages.RemoveAt(0);
            }
            message ="<h3>"+ Context.User.FindFirst(ClaimTypes.Name).Value+ " :  "+message+ "<br>";
            if(u.connectionId == "")
            {
                await SendMyself(m);
            }else{
                await Clients.Client(u.connectionId).InvokeAsync("Send", m);
                await SendMyself(m);
            }
        }
        public async Task SendMyself(MessageModel m)
        {
            await Clients.Client(Context.ConnectionId).InvokeAsync("SendMyself", m);
        }
        

    }
}
