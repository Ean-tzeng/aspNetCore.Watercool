using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WaterCool.Models
{
    public class ChatHub : Hub
    {
        public Task Send(string message)
        {
            message = Context.User.FindFirst(ClaimTypes.Name).Value+ " : " +message+"<br>";
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
