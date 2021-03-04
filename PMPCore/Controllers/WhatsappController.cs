using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;

namespace PMPCore.Controllers
{
    public class WhatsappViewModel
    {
        public int MessageId { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
        public List<string> Attachments { get; set; }
    }

    public class WhatsappController : BaseController
    {
        // GET: Whatsapp
        public IActionResult Index()
        {
            var departments = DepartmentDB.GetInstance().GetAllDepartment();
            return View(departments);
        }

        public JsonResult GetRedirectedMessagesForHeadOfDepartment()
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var list = WhatsappDB.GetInstance().GetRedirectNotifications(personId);


                foreach (var item in list)
                {
                    var attachments = new List<string>();
                    var splitted = item.Message.Split(new string[] { "||=>||" }, StringSplitOptions.None).ToList();

                    for (var i = 0; i < splitted.Count; i++)
                    {
                        if (splitted[i].StartsWith("{\"Key\""))
                        {
                            attachments.Add(splitted[i]);
                            splitted.RemoveAt(i);
                            i = -1;
                        }
                    }

                    for (var i = 0; i < attachments.Count; i++)
                    {
                        if (attachments[i].StartsWith("{\"Key\""))
                        {
                            var jsonModel = JsonConvert.DeserializeObject<KeyValue>(attachments[i]);
                            attachments[i] = $"<a href=\"javascript:downloadMediaMessageById('{item.MessageId}', '{jsonModel.Value.Substring(jsonModel.Value.Length - 30)}');\">{jsonModel.Key.ToUpper().Replace(".", "")} Dosyası</a>";
                        }
                    }

                    item.Message = string.Join(" ", splitted) + (string.Join(" ", splitted).Length > 0 ? "<br>" : "") + string.Join("&nbsp;|&nbsp", attachments);
                }

                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public JsonResult DeleteRedirect(int _id)
        {
            try
            {
                var list = WhatsappDB.GetInstance().DeleteRedirectedMessage(_id);
                return Json(true);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetMilestonesAndSprintsByProjectId(int _projectId)
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var milestones = MilestoneDB.GetInstance().GetAllMilestoneByProjectId(_projectId);
                var sprints = SprintDB.GetInstance().GetAllSprintByProjectId(_projectId);
                var persons = PersonDB.GetInstance().GetHeadOfDepartmentsUsers(personId);

                return Json(new
                {
                    Milestones = milestones.Where(x => x.Status < 3).ToList().OrderBy(x => x.Id).ToList(),
                    Sprints = sprints.Where(x => x.Status < 3).ToList().OrderBy(x => x.Id).ToList(),
                    Persons = persons.OrderBy(x => x.Name).ThenBy(x => x.Surname).ToList()
                });
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SaveWhatsappMessageAsTask(int _redirectId, int _milestoneId, int _sprintId, int _personId, string _title, string _desc)
        {
            try
            {
                if (_desc.Contains("/*") && _desc.Contains("*/"))
                {
                    _desc = _desc.Replace("/*", "<br><i>").Replace("*/", "</i>");
                }
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var result = StepDB.GetInstance().SaveWhatsappTaskAsStep(_redirectId, _milestoneId, _sprintId, personId, _personId, _title, _desc);
                return Json(result != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SaveRedirect(int _messageId, int _departmentId)
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var obj = WhatsappDB.GetInstance().SaveMessageRedirect(_messageId, _departmentId, personId);

                if (obj != null)
                {
                    WhatsappDB.GetInstance().DeleteMessage(_messageId);
                }

                return Json(obj != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteMessage(int _messageId)
        {
            try
            {
                var result = WhatsappDB.GetInstance().DeleteMessage(_messageId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetMessages()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<WhatsappViewModel>();
                    var messages = context.WhatsappMessage.Where(x => x.IsSeen == false).ToList().OrderByDescending(x => x.Id).ToList();

                    foreach (var item in messages)
                    {
                        var attachments = new List<string>();
                        var splitted = item.MessageContent.Split(new string[] { "||=>||" }, StringSplitOptions.None).ToList();

                        for (var i = 0; i < splitted.Count; i++)
                        {
                            if (splitted[i].StartsWith("{\"Key\""))
                            {
                                attachments.Add(splitted[i]);
                                splitted.RemoveAt(i);
                                i = -1;
                            }
                        }

                        for (var i = 0; i < attachments.Count; i++)
                        {
                            if (attachments[i].StartsWith("{\"Key\""))
                            {
                                var jsonModel = JsonConvert.DeserializeObject<KeyValue>(attachments[i]);
                                attachments[i] = $"<a href=\"javascript:downloadMediaMessageById('{item.Id}', '{jsonModel.Value.Substring(jsonModel.Value.Length - 30)}');\">{jsonModel.Key.ToUpper().Replace(".", "")} Dosyası</a>";
                            }
                        }

                        returnList.Add(new WhatsappViewModel()
                        {
                            MessageId = item.Id,
                            Number = item.MessageFrom,
                            Message = string.Join(" ", splitted),
                            Attachments = attachments
                        });
                    }

                    return Json(returnList);



                    //foreach (var item in messages)
                    //{
                    //    if (item.MessageContent.StartsWith("{\"Key\""))
                    //    {
                    //        var jsonModel = JsonConvert.DeserializeObject<KeyValue>(item.MessageContent);
                    //        jsonModel.Value = $"<a href=\"\">{jsonModel.Key.ToUpper().Replace(".", "")} Dosyası</a>";
                    //        item.MessageContent = $"<a href=\"javascript:downloadMediaMessageById('{item.Id}');\">{jsonModel.Key.ToUpper().Replace(".", "")} Dosyası</a>";
                    //    }
                    //}


                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        public JsonResult SendRequest(string _type)
        {
            WhatsappComminucation c = null;

            using (var context = new ProjectManagementEntities())
            {
                c = new WhatsappComminucation()
                {
                    Date = DateTime.Now,
                    Request = _type,
                    Response = null
                };
                context.WhatsappComminucation.Add(c);
                context.SaveChanges();
            }

            while (c.Response == null)
            {
                using (var context = new ProjectManagementEntities())
                {
                    c = context.WhatsappComminucation.FirstOrDefault(x => x.Id == c.Id);
                }
            }

            if (!c.Response.StartsWith("data:image/png;base64") && c.Response != "true")
            {
                var model = JsonConvert.DeserializeObject<List<Whatsapp>>(c.Response);

                using (var context = new ProjectManagementEntities())
                {
                    foreach (var item in model)
                    {
                        item.SonMesaj = HttpUtility.HtmlDecode(item.SonMesaj);
                        item.Gonderen = HttpUtility.HtmlDecode(item.Gonderen);

                        var lastMesaj = context.WhatsappMessage.Where(x => x.MessageFrom == item.Gonderen && x.IsSeen == false).ToList().OrderByDescending(x => x.Id).ToList().FirstOrDefault();

                        if (lastMesaj != null)
                        {
                            if ((DateTime.Now - lastMesaj.Date).TotalMinutes <= 5)
                            {
                                if (!lastMesaj.MessageContent.Contains(item.SonMesaj))
                                {
                                    lastMesaj.MessageContent = lastMesaj.MessageContent + "||=>||" + item.SonMesaj;
                                    lastMesaj.Date = DateTime.Now;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                var allMessages = string.Join("||=>||", context.WhatsappMessage.Where(x => x.MessageFrom == item.Gonderen).Select(x => x.MessageContent));

                                if (allMessages == item.SonMesaj || allMessages.Contains("||=>||" + item.SonMesaj) || allMessages.Contains(item.SonMesaj + "||=>||"))
                                {

                                }
                                else
                                {
                                    var insertObj = new WhatsappMessage()
                                    {
                                        Date = DateTime.Now,
                                        IsSeen = false,
                                        MessageContent = item.SonMesaj,
                                        MessageFrom = item.Gonderen
                                    };
                                    context.WhatsappMessage.Add(insertObj);
                                    context.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            var allMessages = string.Join("||=>||", context.WhatsappMessage.Where(x => x.MessageFrom == item.Gonderen).Select(x => x.MessageContent));

                            if (allMessages == item.SonMesaj || allMessages.Contains("||=>||" + item.SonMesaj) || allMessages.Contains(item.SonMesaj + "||=>||"))
                            {

                            }
                            else
                            {
                                var insertObj = new WhatsappMessage()
                                {
                                    Date = DateTime.Now,
                                    IsSeen = false,
                                    MessageContent = item.SonMesaj,
                                    MessageFrom = item.Gonderen
                                };
                                context.WhatsappMessage.Add(insertObj);
                                context.SaveChanges();
                            }
                        }
                        //var controlList = context.WhatsappMessage.Where(x => x.MessageFrom == item.Gonderen).ToList().OrderBy(x => x.Id).ToList();

                        //if (controlList.Count > 0)
                        //{
                        //    var lastItem = controlList.LastOrDefault();

                        //    if (lastItem.MessageContent == item.SonMesaj && lastItem.MessageFrom == item.Gonderen)
                        //        continue;
                        //}
                    }
                }
            }

            return Json(c.Response);
        }

        public JsonResult DownloadMediaMessageById(int _id, string _last30)
        {
            using (var context = new ProjectManagementEntities())
            {
                _last30 += "\"}";
                var message = context.WhatsappMessage.FirstOrDefault(x => x.Id == _id);
                var obj = message.MessageContent.Split(new string[] { "||=>||" }, StringSplitOptions.None).FirstOrDefault(x => x.StartsWith("{\"Key\"") && x.EndsWith(_last30));

                if (obj == null)
                    return Json("false");

                var json = JsonConvert.DeserializeObject<KeyValue>(obj);


                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssf") + json.Key;
                //DÜZELT
                //var savePath = System.IO.Path.Combine(Server.MapPath("~"), "WhatsappMedia", fileName);
                //var downloadPath = System.IO.Path.Combine(Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, ""), "WhatsappMedia", fileName);
                var savePath = "";
                var downloadPath = "";

                byte[] bytes = Convert.FromBase64String(json.Value);
                System.IO.File.WriteAllBytes(savePath, bytes);
                return Json(downloadPath);
            }
        }
    }

    public class Whatsapp
    {
        public string Gonderen { get; set; }
        public string Tarih { get; set; }
        public string Resim { get; set; }
        public string SonMesaj { get; set; }
    }

    public class KeyValue
    {
        public KeyValue(string _k, string _v)
        {
            this.Key = _k;
            this.Value = _v;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}