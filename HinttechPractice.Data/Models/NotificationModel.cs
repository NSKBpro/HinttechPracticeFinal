using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Data.Models
{
    /// <summary>
    /// Help model for sending notification model over session.
    /// </summary>
    public class NotificationModel
    {
        public int NotificationId { get; set; }

        public String RecipientUsername { get; set; }

        public String SenderUsername { get; set; }

        public DateTime DateCreated { get; set; }

        public String Description { get; set; }

        public Boolean IsRead { get; set; }

        public Boolean IsMessage { get; set; }
    }
}
