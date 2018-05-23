using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AgProgramlamaOdev2.Models;
using AgProgramlamaOdev2.Models.DbModels;
using AgProgramlamaOdev2.Models.ViewModel;
using Microsoft.AspNet.SignalR;

namespace AgProgramlamaOdev2
{
    public class Messanger : Hub
    {
        DatabaseContext databaseContext = new DatabaseContext();

        public void Hello()
        {
            Clients.All.hello();
        }

        public void OnlineOl(int userId)
        {

            User user = databaseContext.Users.Where(x => x.id == userId).FirstOrDefault();
            if (user != null)
            {
                Online onlineUser = databaseContext.Onlines.Where(x => x.User.id == userId).FirstOrDefault();

                if (onlineUser == null)
                {
                    databaseContext.Onlines.Add(new Online()
                    {
                        ConenctionId = Context.ConnectionId,
                        User = user
                    });

                    databaseContext.SaveChanges();
                }
                else
                {
                    onlineUser.ConenctionId = Context.ConnectionId;
                    databaseContext.SaveChanges();
                }
            }


        }

        public void SendPrivateMessage(int senderId, int receiverId, string message)
        {
            User receiverUser = databaseContext.Users.Where(x => x.id == receiverId).FirstOrDefault();
            User senderUser = databaseContext.Users.Where(x => x.id == senderId).FirstOrDefault();

            DateTime mesajGondermeTarihi = DateTime.Now;

            PrivateMessage message2 = new PrivateMessage()
            {
                GetUser = receiverUser,
                SendUser = senderUser,
                SendTime = mesajGondermeTarihi,
                Status = false,
                Message = message,
            };

            PrivateMessage privateMessage = databaseContext.PrivateMessages.Add(message2);
            databaseContext.SaveChanges();

            PrivateMessage_sade privateMessage_Sade_Karsiya = new PrivateMessage_sade()
            {

                id = privateMessage.id,
                Message = privateMessage.Message,
                UserId = privateMessage.SendUser.id,
                UserMail = privateMessage.SendUser.email,
                SendTime = privateMessage.SendTime

            };

            PrivateMessage_sade privateMessage_Sade_Kendine = new PrivateMessage_sade()
            {

                id = privateMessage.id,
                Message = privateMessage.Message,
                UserId = privateMessage.SendUser.id,
                UserMail = privateMessage.SendUser.email,
                SendTime = privateMessage.SendTime

            };

            Clients.Caller.SendMessageSuccess(privateMessage_Sade_Kendine, receiverUser.id);


            Online online = databaseContext.Onlines.Where(x => x.User.id == receiverId).FirstOrDefault();


            if (online != null)
            {
                Clients.Client(online.ConenctionId).PrivateMessageArrived(privateMessage_Sade_Karsiya);
            }


        }
        public void MessageSeen(int receiverId, int senderId)
        {
            List<PrivateMessage> mesajlar = databaseContext.PrivateMessages.Where(x => x.SendUser.id == senderId && x.GetUser.id == receiverId).ToList();

            foreach (var item in mesajlar)
            {
                item.Status = true;
            }
            databaseContext.SaveChanges();
        }
        public void SendGroupMessage(int senderId, int receiverId, string message)
        {
            Group receiverUser = databaseContext.Groups.Where(x => x.id == receiverId).FirstOrDefault();
            User senderUser = databaseContext.Users.Where(x => x.id == senderId).FirstOrDefault();

            DateTime mesajGondermeTarihi = DateTime.Now;

            GroupMessage message2 = new GroupMessage()
            {
                SendUser = senderUser,
                SendTime = mesajGondermeTarihi,
                Message = message,
                Group = receiverUser
            };

            GroupMessage groupMessage = databaseContext.GroupMessages.Add(message2);
            databaseContext.SaveChanges();

            GroupMessage_sade groupMessage_Sade = new GroupMessage_sade()
            {

                id = groupMessage.id,
                Message = groupMessage.Message,
                UserId = groupMessage.SendUser.id,
                UserMail = groupMessage.SendUser.email,
                SendTime = groupMessage.SendTime

            };


            Clients.Group("group_" + receiverId.ToString()).GroupMessageArrived(groupMessage_Sade, receiverUser.id,receiverUser.GroupName);

        }

        public void sendBroadcastMessage(int senderId, string message)
        {
            User senderUser = databaseContext.Users.Where(x => x.id == senderId).FirstOrDefault();
            List<User> users = databaseContext.Users.ToList();

            DateTime mesajGondermeTarihi = DateTime.Now;

            users.Remove(senderUser);

            foreach (var user in users)
            {
                PrivateMessage message2 = new PrivateMessage()
                {
                    GetUser = user,
                    SendUser = senderUser,
                    SendTime = mesajGondermeTarihi,
                    Status = false,
                    Message = message,
                };

                PrivateMessage privateMessage = databaseContext.PrivateMessages.Add(message2);
                databaseContext.SaveChanges();

                PrivateMessage_sade privateMessage_Sade = new PrivateMessage_sade()
                {

                    id = privateMessage.id,
                    Message = privateMessage.Message,
                    UserId = privateMessage.SendUser.id,
                    UserMail = privateMessage.SendUser.email,
                    SendTime = privateMessage.SendTime

                };

                Clients.Caller.SendMessageSuccess(privateMessage_Sade, user.id);

                Online online = databaseContext.Onlines.Where(x => x.User.id == user.id).FirstOrDefault();


                if (online != null)
                {
                    Clients.Client(online.ConenctionId).PrivateMessageArrived(privateMessage_Sade);
                }
            }                 
        }



        public void JoinGroup(int groupId)
        {
            Groups.Add(Context.ConnectionId, "group_" + groupId.ToString());
        }
        public void LeaveGroup(int groupId)
        {
            Groups.Remove(Context.ConnectionId, "group_" + groupId.ToString());
        }



        public override Task OnDisconnected(bool stopCalled)
        {
            Online onlineUser = databaseContext.Onlines.Where(x => x.ConenctionId == Context.ConnectionId).FirstOrDefault();
            if (onlineUser != null)
            {
                databaseContext.Onlines.Remove(onlineUser);
                databaseContext.SaveChanges();
            }

            return base.OnDisconnected(stopCalled);
        }
        public override Task OnConnected()
        {

            return base.OnConnected();
        }
    }
}