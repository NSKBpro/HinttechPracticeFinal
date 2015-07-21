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
            Clients.User(recipientName).addNewMessageToPage(name, message);
           // Clients.All.addNewMessageToPage(name, message);
        }


        /*private List<User> usersLoggedIn = new List<User>();
        private List<User> users = new List<User>();
        public void SendPrivateMessage(string toUserId, string message)
        {
            User u1 = new User();
            u1.id = "1";
            u1.username = "luka";
            u1.password = "luka";
            User u2 = new User();
            u2.id = "2";
            u2.username = "meda";
            u2.password = "meda";
            users.Add(u1);
            users.Add(u2);

            string fromUserId = Context.ConnectionId;

            var toUser = u1;
            var fromUser = u2;

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.username, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.username, message);
            }
        }*/
    }
}