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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public int CreatedBy { get; set; }
        public int SentTo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
        public string Description { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }
    }
}
