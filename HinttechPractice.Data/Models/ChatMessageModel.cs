using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Data.Models
{
    /// <summary>
    /// Help model for sending chatMessage over session.
    /// </summary>
    public class ChatMessageModel
    {
        public String Sender { get; set; }

        public String Recipient { get; set; }

        public DateTime DateCreated { get; set; }

        public String Message { get; set; }
    }
}
