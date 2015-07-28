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
        /// <summary>
        /// Send notification to all online users when admin change/edit/delete holiday.
        /// </summary>
        /// <param name="type">add, edit, delete</param>
        /// <param name="message">Holiday description.</param>
        /// <param name="sender">Who change/edit/delete holiday.</param>
        public void SendNotification(string type, string message, string sender)
        {
            NotificationService notificationService = new NotificationService();
            UsersService userService = new UsersService();

            User createdBy = (User)userService.FindUserByUsername(sender);
            int userCount = 0;
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
                    userCount++;
                }
            }
            int maxNotificationId = notificationService.FindLastNotificationId();
            int minNotificationId = maxNotificationId - userCount;

            Clients.All.broadcastNotification(type, message, minNotificationId, maxNotificationId);
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

        /// <summary>
        /// Initialization method to fill all notification for user.
        /// </summary>
        public void SendNotification()
        {
            NotificationService notificationService = new NotificationService();
            notificationService.FixNotificationSpam();
            List<NotificationModel> previousNotification = notificationService.AllUnreadNotification();
            Clients.All.loadNotificationList(previousNotification);
        }

        /// <summary>
        /// Find notification by notificationId and change state isRead to true;
        /// </summary>
        /// <param name="notificationId">Current notification id</param>
        public void MarkAsRead(int notificationId)
        {
            NotificationService notificationService = new NotificationService();
            Notification notification = (Notification)notificationService.FindById(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notificationService.Edit(notification);
            }
        }

        /// <summary>
        /// Find notification in range when someone change holiday(send one message to n peole, in range),
        /// and with his username, change isReady to true.
        /// </summary>
        /// <param name="currentUsername">Username of user who clicked.</param>
        /// <param name="minNotificationId">min range notification.</param>
        /// <param name="maxNotificationId">max range notification.</param>
        public void MarkAsReadHolidayNotifications(string currentUsername, int minNotificationId, int maxNotificationId)
        {
            NotificationService notificationService = new NotificationService();
            Notification notification = (Notification)notificationService.FindCurrentUserNotificationInRange(currentUsername, minNotificationId, maxNotificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notificationService.Edit(notification);
            }
        }
    }

}