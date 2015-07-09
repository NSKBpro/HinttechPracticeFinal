using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HinttechPractice.Models
{
    public class UserViewModel
    {
        [Key]
        [Display(Name = "Username:")]
        public string Username { get; set; }

        [Display(Name = "First name:")]
        public string FirstName { get; set; }

        [Display(Name = "Last name:")]
        public string LastName { get; set; }

        [Display(Name = "Email:")]
        public string Email { get; set; }

        [Display(Name = "Registered:")]
        public bool IsUserRegistered { get; set; }

        [Display(Name = "Picture:")]
        public byte[] ProfilePicture { get; set; }

        [Display(Name = "Date created:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateCreated { get; set; }

        [Display(Name = "Vacation days:")]
        public int VacationDays { get; set; }

    }
}