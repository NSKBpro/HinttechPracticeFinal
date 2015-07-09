using HinttechPractice.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HinttechPractice.Models
{
    public class Holiday
    {
        public int HolidayId { get; set; }
        public int UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public String Descrition { get; set; }
        
    }
}