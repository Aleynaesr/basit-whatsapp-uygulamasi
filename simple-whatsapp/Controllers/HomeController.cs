using AgProgramlamaOdev2.Models;
using AgProgramlamaOdev2.Models.DbModels;
using AgProgramlamaOdev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgProgramlamaOdev2.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext databaseContext = new DatabaseContext();
        public ActionResult Index()
        {
            if (Session["session"] != null)
            {

                string email = Session["session"].ToString();
                User user = databaseContext.Users.Where(x => x.email == email).FirstOrDefault();

                List<User> users = databaseContext.Users.ToList();
                List<Group> groups = databaseContext.Groups.ToList();

                users.Remove(user);

                List<UsersNotSeenMessage> usersNotSeenMessages = new List<UsersNotSeenMessage>();
                string deneme;

                for (int i = 0; i < users.Count; i++)
                {
                    deneme = users[i].email;

                    int privateMessagesCount = databaseContext.PrivateMessages.Where(x => x.Status == false && x.GetUser.email == email && x.SendUser.email== deneme).Count();

                    UsersNotSeenMessage usersNotSeenMessage = new UsersNotSeenMessage()
                    {
                        id = users[i].id,
                        MessageCount = privateMessagesCount

                    };

                    usersNotSeenMessages.Add(usersNotSeenMessage);
                }


                var JsonObject = new
                {
                    users = users.Select(x => new
                    {
                        id = x.id,
                        email = x.email,
                        gorummemisMesajSayisi =(int)usersNotSeenMessages.Where(k => k.id == x.id).Select(k => k.MessageCount).FirstOrDefault()
                    }),
                    groups=groups.Select(x=> new
                    {
                        id=x.id,
                        groupName=x.GroupName,                       
                    }),
                    sessionId=user.id,
                    sessionMail=user.email
                };

                return View(JsonObject);
            }

            return RedirectToAction("Login", "Account");
        }



        public JsonResult GetPrivateMessages(int userId)
        {
            string email = Session["session"].ToString();

            List<PrivateMessage> privateMessages = databaseContext.PrivateMessages.Where(x => (x.SendUser.email == email && x.GetUser.id==userId) || (x.SendUser.id == userId && x.GetUser.email==email)).ToList();

            for (int i = 0; i < privateMessages.Count; i++)
            {
                if(privateMessages[i].Status==false)
                {
                    privateMessages[i].Status = true;
                }       
            }
            databaseContext.SaveChanges();

            List<PrivateMessage_sade> privateMessage_Sade = privateMessages.Select(x => new PrivateMessage_sade
            {
                id=x.id,
                Message=x.Message,
                SendTime=x.SendTime,
                UserId=x.SendUser.id,
                UserMail=x.SendUser.email

            }).ToList();


            var JsonObject = new
            {
                messages = privateMessage_Sade,
            };


            return Json(JsonObject, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetGroupMessages(int groupId)
        {
            string email = Session["session"].ToString();

            List<GroupMessage> groupMessages = databaseContext.GroupMessages.Where(x => x.Group.id==groupId).ToList();


            List<GroupMessage_sade> GroupMessage_sade = groupMessages.Select(x => new GroupMessage_sade
            {
                id = x.id,
                Message = x.Message,
                SendTime = x.SendTime,
                UserId = x.SendUser.id,
                UserMail = x.SendUser.email

            }).ToList();


            var JsonObject = new
            {
                messages = GroupMessage_sade,
            };


            return Json(JsonObject, JsonRequestBehavior.AllowGet);

        }
    }
}