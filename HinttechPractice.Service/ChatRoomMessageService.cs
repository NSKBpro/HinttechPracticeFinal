using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Data.Models;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service
{
    /// <summary>
    /// DAL class for communication with database.
    /// </summary>
    public class ChatRoomMessageService : ICommonOp
    {
        private readonly DataContext context;

        public ChatRoomMessageService()
        {
            if (context == null)
            {
                context = new DataContext();
            }
        }

        public void Create(object messageObject)
        {
            context.ChatRoomMessages.Add((ChatRoomMessage)messageObject);
            context.SaveChanges();
        }

        public object FindById(int messageId)
        {
            ChatRoomMessage message = new ChatRoomMessage();
            message = context.ChatRoomMessages.Find(messageId);
            return message;
        }

        public List<ChatRoomMessage> FindAll()
        {
            return context.ChatRoomMessages.ToList();
        }

        /// <summary>
        /// Find all messages for current room.
        /// </summary>
        /// <param name="roomId">Current room Id.</param>
        /// <returns>List of all messages in that room.</returns>
        public List<ChatMessageModel> FindAllMessagesForCurrentRoom(int roomId)
        {
            List<ChatMessageModel> messages = new List<ChatMessageModel>();

            var messageFromChatRoom = from room in context.ChatRoomMessages
                                      where room.RoomId == roomId
                                      select room;

            List<ChatRoomMessage> msgFromDB = messageFromChatRoom.ToList();

            foreach (ChatRoomMessage message in msgFromDB)
            {
                ChatMessageModel newMessage = new ChatMessageModel();
                String sender = context.Users.Find(message.CreatedBy).Username;
                String recipient = null;
                if (message.SentTo != null)
                {
                    recipient = context.Users.Find(message.SentTo).Username;
                    newMessage.Recipient = recipient;
                }

                newMessage.Sender = sender;
                newMessage.Message = message.Message;
                newMessage.DateCreated = message.DateCreated;

                messages.Add(newMessage);
            }
            return messages;
        }




        public void Delete(int messageId)
        {
            throw new NotImplementedException();
        }

        public void Edit(object messageObject)
        {
            throw new NotImplementedException();
        }
    }
}
