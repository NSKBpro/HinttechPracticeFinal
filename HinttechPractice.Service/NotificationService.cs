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

            var notificationsDb = from notification in context.Notifications
                                  where notification.IsRead == false
                                  select notification;

            List<Notification> notificationsFromDB = notificationsDb.ToList();
            foreach (Notification notification in notificationsFromDB)
            {
                NotificationModel notificationModel = new NotificationModel();
                String recipientUsername = ((User)context.Users.Find(notification.SentTo)).Username;
                String senderUsername = ((User)context.Users.Find(notification.CreatedBy)).Username;

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
            return notifications;
        }

        /// <summary>
        /// If one user recive two message with same description, it will count only the last one as
        /// new notification.
        /// </summary>
        public void FixNotificationSpam()
        {
            List<Notification> notifications = context.Notifications.ToList();
            notifications = notifications.OrderBy(n => n.CreatedBy).ThenBy(n => n.SentTo).ToList();

            for (int i = 0; i < notifications.Count(); i++)
            {
                if (i == 0)
                {
                    continue;
                }
                else
                {
                    if (notifications.ElementAt(i - 1).Description.Equals(notifications.ToList().ElementAt(i).Description)
                        && notifications.ElementAt(i - 1).SentTo == notifications.ElementAt(i).SentTo)
                    {
                        notifications.ElementAt(i - 1).IsRead = true;
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
