//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HinttechPractice.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class User
    {
        public User()
        {
            this.ChatRoomMessages = new HashSet<ChatRoomMessage>();
            this.ChatRooms = new HashSet<ChatRoom>();
            this.Holidays = new HashSet<Holiday>();
            this.Vacations = new HashSet<Vacation>();
        }
    
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsUserRegistered { get; set; }
        public bool IsUserAdmin { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public int VacationDays { get; set; }
    
        public virtual ICollection<ChatRoomMessage> ChatRoomMessages { get; set; }
        public virtual ICollection<ChatRoom> ChatRooms { get; set; }
        public virtual ICollection<Holiday> Holidays { get; set; }
        public virtual ICollection<Vacation> Vacations { get; set; }
    }
}
