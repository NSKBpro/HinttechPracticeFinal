using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HinttechPractice.Service;
using HinttechPractice.Data;
using System.Web.Script.Serialization;
using HinttechPractice.Data.Models;

namespace HinttechPractice.Hubs
{
    public class ChatHub : Hub
    {
        List<ChatMessageModel> previousMessages = new List<ChatMessageModel>();
        public void Send(string name, string message, string recipientName)
        {
            ChatRoomMessageService messageService = new ChatRoomMessageService();
            UsersService userService = new UsersService();
            ChatRoomsService roomService = new ChatRoomsService();


            if (recipientName != null)
            {
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage();
                User sender = userService.FindUserByUsername(name);
                User recipient = userService.FindUserByUsername(recipientName);
                if (sender != null && recipient != null)
                {
                    chatRoomMessage.CreatedBy = sender.UserId;
                    chatRoomMessage.DateCreated = DateTime.Now;
                    chatRoomMessage.Message = message;
                    chatRoomMessage.SentTo = recipient.UserId;
                    int roomId = roomService.FindRoomIdForPrivateChat(chatRoomMessage);
                    if (roomId != -1)
                    {
                        chatRoomMessage.RoomId = roomId;
                    }
                    else
                    {
                        ChatRoom chatRoom = new ChatRoom();
                        chatRoom.CreatedBy = sender.UserId;
                        chatRoom.DateCreated = DateTime.Now;
                        chatRoom.IsDeleted = false;
                        chatRoom.RoomDescription = "Private chat";
                        chatRoom.RoomName = "Private_Chat";
                        chatRoom.IsPrivate = true;
                        int currentRoomId = roomService.CreateWithId(chatRoom);
                        chatRoomMessage.RoomId = currentRoomId;
                    }
                    messageService.Create(chatRoomMessage);
                    previousMessages = messageService.FindAllMessagesForCurrentRoom(chatRoomMessage.RoomId);
                    Clients.User(recipientName).addNewMessageToPage(name, message, recipientName, previousMessages);
                }
            }
            else
            {
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage();
                User sender = userService.FindUserByUsername(name);
                if (sender != null)
                {
                    chatRoomMessage.CreatedBy = sender.UserId;
                    chatRoomMessage.DateCreated = DateTime.Now;
                    chatRoomMessage.Message = message;
                    chatRoomMessage.SentTo = null;
                    chatRoomMessage.RoomId = 1; // MASTER ROOM

                    messageService.Create(chatRoomMessage);
                    previousMessages = messageService.FindAllMessagesForCurrentRoom(chatRoomMessage.RoomId);
                    Clients.All.addNewMessageToPage(name, message, recipientName, previousMessages);
                }
            }
        }

        public void SendMessageNotification(string name, string message, string recipientName)
        {
            NotificationService notificationService = new NotificationService();
            UsersService userService = new UsersService();

            User createdBy = (User)userService.FindUserByUsername(name);
            User sentTo = (User)userService.FindUserByUsername(recipientName);

            Notification notification = new Notification();
            notification.DateCreated = DateTime.Now;
            notification.CreatedBy = createdBy.UserId;
            notification.Description = "New message from " + name;
            notification.IsRead = false;
            notification.SentTo = sentTo.UserId;
            notificationService.Create(notification);

            Clients.User(recipientName).broadcastNotification(name, message, recipientName);
        }

        public void InitialCheckUp()
        {

            ChatRoomMessageService messageService = new ChatRoomMessageService();
            ChatRoomsService roomService = new ChatRoomsService();
            List<ChatMessageModel> previousMessages = new List<ChatMessageModel>();
            ChatRoomMessage chatRoomMessage = new ChatRoomMessage();

            previousMessages = messageService.FindAllMessagesForCurrentRoom(1);
            Clients.All.addNewMessageToPageInitial(previousMessages);
        }

        public void LoadPrivateMessagesHistory(string name, string recipientName)
        {
            ChatRoomsService chatRoomsService = new ChatRoomsService();
            IList<ChatMessageModel> previousMessages;
            previousMessages = chatRoomsService.LoadPrivateMessagesHistory(name, recipientName);

            Clients.User(recipientName).loadLastMessages(previousMessages);
        }
    }
}
