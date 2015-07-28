using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Data.Models;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service
{
    /// <summary>
    /// DAL class for communication with database.
    /// </summary>
    public class NotificationService : ICommonOp
    {
        private readonly DataContext context;

        public NotificationService()
        {
            if (context == null)
            {
                context = new DataContext();
            }
        }

        public void Create(object notificationObject)
        {
            context.Notifications.Add((Notification)notificationObject);
            context.SaveChanges();
        }

        public object FindById(int notificationId)
        {
            Notification notification = new Notification();
            notification = context.Notifications.Find(notificationId);
            return notification;
        }

        public List<Notification> FindAll()
        {
            return context.Notifications.ToList();
        }

        public void Delete(int notificationId)
        {
            Notification notification = context.Notifications.Find(notificationId);
            if (notification != null)
            {
                context.Notifications.Remove(notification);
            }
        }

        public void Edit(object notification)
        {
            Notification oldNotification = context.Notifications.Find(((Notification)notification).NotificationId);
            if (oldNotification != null)
            {
                context.Entry(oldNotification).CurrentValues.SetValues((Notification)notification);
                context.Entry(oldNotification).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Return all unread notification.
        /// </summary>
        /// <returns>List of all unread notification.</returns>
        public List<NotificationModel> AllUnreadNotification()
        {
            List<NotificationModel> notifications = new List<NotificationModel>();
            String recipientUsername = null;
            String senderUsername = null;
            foreach (Notification notification in context.Notifications)
            {
                if (!notification.IsRead)
                {
                    NotificationModel notificationModel = new NotificationModel();
                    using (var userContext = new DataContext())
                    {
                        recipientUsername = ((User)userContext.Users.Find(notification.SentTo)).Username;
                        senderUsername = ((User)userContext.Users.Find(notification.CreatedBy)).Username;
                    }
                    notificationModel.DateCreated = notification.DateCreated;
                    notificationModel.Description = notification.Description;
                    notificationModel.IsRead = true;
                    notificationModel.RecipientUsername = recipientUsername;
                    notificationModel.SenderUsername = senderUsername;
                    notificationModel.NotificationId = notification.NotificationId;
                    if (notification.Description.Contains("New message from"))
                    {
                        notificationModel.IsMessage = true;
                    }
                    else
                    {
                        notificationModel.IsMessage = false;
                    }
                    notifications.Add(notificationModel);
                }
            }
            return notifications;
        }

        /// <summary>
        /// If one user recive two message with same description, it will count only the last one as
        /// new notification.
        /// </summary>
        public void FixNotificationSpam()
        {
            for (int i = 0; i < context.Notifications.ToList().Count(); i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else
                {
                    if (context.Notifications.ToList().ElementAt(i - 1).Description.Equals(context.Notifications.ToList().ElementAt(i).Description)
                        && context.Notifications.ToList().ElementAt(i - 1).SentTo == context.Notifications.ToList().ElementAt(i).SentTo)
                    {
                        context.Notifications.ToList().ElementAt(i - 1).IsRead = true;
                    }
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Find max notificationId
        /// </summary>
        /// <returns> max notificationId - last one</returns>
        public int FindLastNotificationId()
        {
            return context.Notifications.OrderByDescending(u => u.NotificationId).FirstOrDefault().NotificationId;
        }

        /// <summary>
        /// Find notification in range for current user.
        /// </summary>
        /// <param name="currentUsername">Username of logged user.</param>
        /// <param name="minNotificationId">Max - notifcationMessage sent count.</param>
        /// <param name="maxNotificationId">Max notification id : last</param>
        /// <returns>My notification.</returns>
        public Notification FindCurrentUserNotificationInRange(string currentUsername, int minNotificationId, int maxNotificationId)
        {
            Notification notification = null;
            UsersService userService = new UsersService();
            while (minNotificationId <= maxNotificationId)
            {
                notification = context.Notifications.Find(minNotificationId);
                User user = userService.FindUserByUsername(currentUsername);
                if (user != null)
                {
                    if (user.UserId == notification.SentTo)
                    {
                        return notification;
                    }
                }
                minNotificationId++;
            }
            return notification;

        }
    }
}
