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

        public void LoadPrivateMessagesHistory(string name, string recipientName)
        {
            ChatRoomsService chatRoomsService = new ChatRoomsService();
            IList<ChatMessageModel> previousMessages;
            previousMessages = chatRoomsService.LoadPrivateMessagesHistory(name, recipientName);

            Clients.User(recipientName).broadcastNotification(previousMessages);
        }

    
    }
}