using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WaterCool.Models
{
    public class ChatHub : Hub
    {
        public Task Send(string message)
        {
            return Clients.All.InvokeAsync("Send", message);
        }
    }
}
