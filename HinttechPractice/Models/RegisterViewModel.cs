using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HinttechPractice.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "Username:")]
        [Required(ErrorMessage = "Enter valid username!", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Enter valid password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password:")]
        [Required(ErrorMessage = "Confirm password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string repeatPassword { get; set; }

        [Display(Name = "First name:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Display(Name = "Last name:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Picture(optional):")]
        [DataType(DataType.Upload)]
        public byte[] ProfilePicture { get; set; }

        public HttpPostedFileBase File { get; set; }
    }

    public class EditAccountViewModel
    {
        [Display(Name = "First name:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        public string FirstName { get; set; }

        [Display(Name = "Last name:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        public string LastName { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Can't be empty.", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
       
        [Display(Name = "Remove image:")]
        public Boolean removeImage { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Display(Name = "Current password:")]
        [Required(ErrorMessage = "Enter current password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string currentPassword { get; set; }

        [Display(Name = "New password:")]
        [Required(ErrorMessage = "Enter new password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }


        [Display(Name = "Confirm new password:")]
        [Required(ErrorMessage = "Confirm new password!", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string confirmNewPass { get; set; }
    }
}