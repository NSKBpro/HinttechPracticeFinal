using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;

namespace HinttechPractice.Hubs
{
    public class NotifierHub : Hub
    {
        public void SendNotification(string type, string message)
        {
            Clients.All.broadcastNotification(type, message);
        }

    
    }
}