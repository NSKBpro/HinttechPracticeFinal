using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace HinttechPractice.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string recipientName)
        {
            if (recipientName != null)
            {
                Clients.User(recipientName).addNewMessageToPage(name, message, recipientName);
            }
            else
            {
                Clients.All.addNewMessageToPage(name, message, recipientName);
            }
        }

        public void SendMessageNotification(string name, string message, string recipientName)
        {
            Clients.User(recipientName).broadcastNotification(name, message, recipientName);
        }

    }
}