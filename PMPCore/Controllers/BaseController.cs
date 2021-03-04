using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace PMPCore.Controllers
{

    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Session.GetString("ActivePerson") == null)
            {
                filterContext.Result = new RedirectResult("/Login");
                return;
            }

            ViewBag.ActivePerson = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson"));
            var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
            ViewBag.Projects = ProjectDB.GetInstance().GetAllProject(personId);

            if (HttpContext.Session.GetString("SelectedProject") != null)
            {
                ViewBag.SelectedProjectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                ViewBag.SelectedProjectName = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Name;
            }
            else
            {
                ViewBag.SelectedProjectId = 0;
            }
            //Session.Timeout = 520000;

            //      var user = Session["ActiveUser"] as Kullanici;

            //      if (user == null)
            //      {

            //          filterContext.Result = new RedirectResult("/Login");
            //          return;
            //      }

            base.OnActionExecuting(filterContext);
        }

        public void ChangeSelectedProject(int _projectId)
        {
            var project = ProjectDB.GetInstance().GetProjeById(_projectId);

            if (project != null)
            {
                HttpContext.Session.SetString("SelectedProject", JsonConvert.SerializeObject(project));
            }
        }

        public JsonResult MakeMessagesSeen(int _messageId)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                MessageDB.GetInstance().MakeMessagesSeen(_messageId, userId);
                return Json("");
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllPersonForMessageSend()
        {
            try
            {
                var users = PersonDB.GetInstance().GetAllPerson();

                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var control = users.FirstOrDefault(x => x.Id == userId);

                if (control != null)
                    users.Remove(control);

                return Json(users.OrderBy(x => x.Name).ThenBy(x => x.Surname).ToList());
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetUnreadMessageCount()
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var count = MessageDB.GetInstance().GetUnreadMessageCount(userId);

                return Json(count);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetNormalMessages()
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var list = MessageDB.GetInstance().GetMainMessageList(userId);

                return Json(list);

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetMessagesBetweenTwoPeople(int _messageId)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var list = _messageId == 0 ? new List<MessageRepo>() : MessageDB.GetInstance().GetMessagesBetweenTwoPeople(_messageId, userId);

                return Json(list);

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public void SendEmail(string message, List<string> _emailToSendReport)
        {
            var temp = System.IO.File.ReadAllText("./wwwroot/mailTemplate.html");
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.gmail.com";
            sc.EnableSsl = true;
            sc.UseDefaultCredentials = false;
            sc.Credentials = new NetworkCredential("merveeozlemm@gmail.com", "merve1998");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("merveeozlemm@gmail.com", "IntellityAI");
            for (int i = 0; i < _emailToSendReport.Count; i++)
            {
                mail.To.Add(_emailToSendReport[i]);
            }
            mail.Subject = "PMP";
            mail.IsBodyHtml = true;
            mail.Body =temp.Replace("mesaj",message);
            sc.Timeout = Int32.MaxValue;
            sc.Send(mail);
        }
        public JsonResult SendMessage(int _lastMessageId, string _text, int _receiverId)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var result = MessageDB.GetInstance().SendMessage(_lastMessageId, userId, _text, _receiverId);
                return Json(result != null ? result.Id : 0);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
