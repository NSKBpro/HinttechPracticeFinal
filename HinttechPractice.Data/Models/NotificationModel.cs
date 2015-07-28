using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Data.Models
{
    public class NotificationModel
    {
        public String RecipientUsername { get; set; }

        public DateTime DateCreated { get; set; }

        public String Description { get; set; }

        public Boolean IsRead { get; set; }
    }
}
