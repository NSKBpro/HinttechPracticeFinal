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

    }
}
