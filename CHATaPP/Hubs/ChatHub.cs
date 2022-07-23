using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CHATaPP.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string message)
        {
            Clients.All.SendAsync("receiveMessage", message);
            Clients.Caller.SendAsync("receiveMessage", "your message");
            Clients.Others.SendAsync("receiveMessage", "not your message");
        }

        public void SendMessageToOneUser(string connectionId, string message)
        {
            Clients.Client(connectionId).SendAsync("receiveMessage", message);
            Clients.Caller.SendAsync("receiveMessage", message);
        }

        public void SendGroupMessage(string groupName, string message)
        {
            Clients.Group(groupName).SendAsync("receiveMessage", message);
        }

        public void JoinGroup(string groupName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Clients.Group(groupName).SendAsync("receiveMessage", $"{Context.ConnectionId} joined");
        }

        public override Task OnConnectedAsync()
        {
            Clients.All.SendAsync("receiveMessage", $"{Context.ConnectionId} : connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.All.SendAsync("receiveMessage", $"{Context.ConnectionId} : Disconnected");
            return base.OnDisconnectedAsync(exception);
        }

    }
}
