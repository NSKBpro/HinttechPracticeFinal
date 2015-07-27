using HinttechPractice.Data;
using HinttechPractice.Data.DataContext;
using HinttechPractice.Data.Models;
using HinttechPractice.Service.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Service
{
    /// <summary>
    /// DAL class for communication with database.
    /// </summary>
    public class ChatRoomsService : ICommonOp
    {
        private readonly DataContext context;

        public ChatRoomsService()
        {
            if (context == null)
            {
                context = new DataContext();
            }
        }

        public void Create(object chatRoomObject)
        {
            context.ChatRooms.Add((ChatRoom)chatRoomObject);
            context.SaveChanges();
        }

        /// <summary>
        /// Create new room, for private chat.
        /// </summary>
        /// <param name="chatRoomObject">Room object to create</param>
        /// <returns>Current room object ID.</returns>
        public int CreateWithId(object chatRoomObject)
        {
            context.ChatRooms.Add((ChatRoom)chatRoomObject);
            context.SaveChanges();
            return ((ChatRoom)chatRoomObject).RoomId;
        }

        public object FindById(int chatRoomId)
        {
            ChatRoom chatRoom = new ChatRoom();
            chatRoom = context.ChatRooms.Find(chatRoomId);
            return chatRoom;
        }

        public List<ChatRoom> FindAll()
        {
            return context.ChatRooms.ToList();
        }

        public void Delete(int chatRoomId)
        {
            ChatRoom chatRoom = context.ChatRooms.Find(chatRoomId);
            if (chatRoom != null)
            {
                context.ChatRooms.Remove(chatRoom);
            }
        }

        public IList<ChatMessageModel> LoadPrivateMessagesHistory(string userFrom, string userTo)
        {
            UsersService userService = new UsersService();
            ChatRoomMessageService msgService = new ChatRoomMessageService();
            User sender = userService.FindUserByUsername(userFrom);
            User recipient = userService.FindUserByUsername(userTo);

            ChatRoomMessage messageTemp = new ChatRoomMessage();
            messageTemp.CreatedBy = sender.UserId;
            messageTemp.SentTo = recipient.UserId;

            int roomId = FindRoomIdForPrivateChat(messageTemp);

            if (roomId != -1)
            {
                return msgService.FindAllMessagesForCurrentRoom(roomId).ToList();
            }

            return null;
        }

        public void Edit(object chatRoom)
        {
            ChatRoom oldChatRoom = context.ChatRooms.Find(((ChatRoom)chatRoom).RoomId);
            if (oldChatRoom != null)
            {
                context.Entry(oldChatRoom).CurrentValues.SetValues((ChatRoom)chatRoom);
                context.Entry(oldChatRoom).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Return a RoomId for new message.
        /// </summary>
        /// <param name="currentMessage"> Message for sending.</param>
        /// <returns>RoomId for currentMessage. If value is -1, need to create new room.</returns>
        public int FindRoomIdForPrivateChat(ChatRoomMessage currentMessage)
        {
            foreach (ChatRoomMessage chatMessage in context.ChatRoomMessages)
            {
                if (isExistPreviousConversation(currentMessage, chatMessage))
                {
                    return chatMessage.RoomId;
                }
            }

            return -1;
        }

        /// <summary>
        /// Check does already exist combination of Created-SentTo combination to get RoomId
        /// </summary>
        /// <param name="currentMessage">Message for sending.</param>
        /// <param name="chatMessage">Loop thought all messages in database.</param>
        /// <returns>True if exist, False if isn't exist.</returns>
        private Boolean isExistPreviousConversation(ChatRoomMessage currentMessage, ChatRoomMessage chatMessage)
        {
            if (chatMessage.CreatedBy.Equals(currentMessage.CreatedBy) && chatMessage.SentTo.Equals(currentMessage.SentTo)
                || chatMessage.CreatedBy.Equals(currentMessage.SentTo) && chatMessage.SentTo.Equals(currentMessage.CreatedBy))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
