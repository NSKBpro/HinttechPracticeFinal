using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using System.Collections.Generic;
using HinttechPractice.Service;
using HinttechPractice.Data.Models;
using HinttechPractice.Data;

namespace HinttechPractice.Hubs
{
    public class NotifierHub : Hub
    {
        public void SendNotification(string type, string message, string sender)
        {
            NotificationService notificationService = new NotificationService();
            UsersService userService = new UsersService();

            User createdBy = (User)userService.FindUserByUsername(sender);
            foreach (User user in userService.FindAll())
            {
                if (!user.IsUserAdmin && user.IsUserRegistered)
                {
                    Notification notification = new Notification();
                    notification.CreatedBy = createdBy.UserId;
                    notification.DateCreated = DateTime.Now;
                    notification.IsRead = false;
                    notification.SentTo = user.UserId;
                    notification.Description = ChoseDescription(type, message, sender);
                    notificationService.Create(notification);
                }
            }
            Clients.All.broadcastNotification(type, message);
        }

        /// <summary>
        /// Return text for notification description in database.
        /// </summary>
        /// <param name="type">add, edit, delete</param>
        /// <param name="message">Holiday description</param>
        /// <param name="sender">Who make change/add/delete</param>
        /// <returns>Value of description.</returns>
        private string ChoseDescription(string type, string message, string sender)
        {
            string retVal = "";
            string[] description = message.Split(' ');
            switch (type)
            {
                case "add": retVal = sender + " added new holiday(" + description[1] + ")";
                    break;
                case "delete": retVal = sender + " deleted holiday(" + description[1] + ")";
                    break;
                case "edit": retVal = sender + " edited holiday(" + description[1] + ")";
                    break;
            }
            return retVal;
        }

        public void SendNotification()
        {
            NotificationService notificationService = new NotificationService();
            List<NotificationModel> previousNotification = notificationService.AllUnreadNotification();
            Clients.All.loadNotificationList(previousNotification);
        }

    }

}