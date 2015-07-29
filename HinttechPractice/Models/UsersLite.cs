using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HinttechPractice.Models
{
    public class UsersLite
    {
        public String username { get; set; }
        public int usrId { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public byte[] profilePicture { get; set; }
        public bool activity { get; set; }
        public String lastSeenOn { get; set; }
    }
}