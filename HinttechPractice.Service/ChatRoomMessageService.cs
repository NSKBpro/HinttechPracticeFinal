using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Data.Models;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
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

            foreach (ChatRoomMessage message in context.ChatRoomMessages)
            {
                if (message.RoomId == roomId)
                {
                    ChatMessageModel newMessage = new ChatMessageModel();
                    User sender = null;
                    User recipient = null;
                    using (var userContext = new DataContext())
                    {
                        sender = userContext.Users.Find(message.CreatedBy);
                        if (message.SentTo != null)
                        {
                            recipient = userContext.Users.Find(message.SentTo);
                            newMessage.Recipient = recipient.Username;
                        }
                    }

                    newMessage.Sender = sender.Username;
                    newMessage.Message = message.Message;
                    newMessage.DateCreated = message.DateCreated;

                    messages.Add(newMessage);
                }
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
