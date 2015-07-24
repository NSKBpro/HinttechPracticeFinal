using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
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
        public List<ChatRoomMessage> FindAllMessagesForCurrentRoom(int roomId)
        {
            List<ChatRoomMessage> messages = new List<ChatRoomMessage>();
            foreach (ChatRoomMessage message in context.ChatRoomMessages)
            {
                if (message.RoomId == roomId)
                {
                    messages.Add(message);
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
