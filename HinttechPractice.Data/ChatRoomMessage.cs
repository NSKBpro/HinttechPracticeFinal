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
    
    public partial class ChatRoomMessage
    {
        [Key]
        public int MessageId { get; set; }
        public int RoomId { get; set; }
        public string Message { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DateCreated { get; set; }

        public int CreatedBy { get; set; }
        public Nullable<int> SentTo { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User User { get; set; }

    }
}
