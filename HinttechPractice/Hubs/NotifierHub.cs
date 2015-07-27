using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using System.Collections.Generic;
using HinttechPractice.Service;
using HinttechPractice.Data.Models;

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