using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelAdvisor.Models
{
    ///<summary>
    ///View model for login user.
    ///</summary>
    public class LoginViewModel
    {
        [Display(Name = "Username:")]
        [Required(ErrorMessage="Enter valid username!",AllowEmptyStrings=false)]
        public string UserName { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Enter valid password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}