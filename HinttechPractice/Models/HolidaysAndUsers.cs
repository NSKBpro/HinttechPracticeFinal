using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HinttechPractice.Models
{
    public class HolidaysAndUsers
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public int HolidayId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public String Descrition { get; set; }
    }
}