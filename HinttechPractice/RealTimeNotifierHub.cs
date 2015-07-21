using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;

namespace HinttechPractice
{
    public class RealTimeNotifierHub : Hub
    {
        public void SendNotification(string type, string message)
        {
            Clients.All.broadcastNotification(type, message);
        }

    }
}