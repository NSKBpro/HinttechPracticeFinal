using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using HinttechPractice.Service;
using HinttechPractice.Data;

namespace HinttechPractice.Hubs
{
    public class ChatHub : Hub
    {
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
                    //ViewBag.globalMessage = messageService.FindAllMessagesForCurrentRoom(chatRoomMessage.RoomId);
                    Clients.User(recipientName).addNewMessageToPage(name, message, recipientName);
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
                    //ViewBag.globalMessage = messageService.FindAllMessagesForCurrentRoom(chatRoomMessage.RoomId);
                    Clients.All.addNewMessageToPage(name, message, recipientName);
                }
            }
        }

        public void SendMessageNotification(string name, string message, string recipientName)
        {
            Clients.User(recipientName).broadcastNotification(name, message, recipientName);
        }

    }
}