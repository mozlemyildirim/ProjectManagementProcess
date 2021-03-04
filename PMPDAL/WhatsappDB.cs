using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class WhatsappDB
    {
        private static WhatsappDB instance = null;

        public static WhatsappDB GetInstance()
        {
            if (instance == null)
                instance = new WhatsappDB();
            return instance;
        }

        public bool DeleteMessage(int _messageId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var message = context.WhatsappMessage.FirstOrDefault(x => x.Id == _messageId);

                    if (message != null)
                    {
                        message.IsSeen = true;
                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0;
                    }
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeleteRedirectedMessage(int _redirectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var redirect = context.WhatsappMessageRedirect.FirstOrDefault(x => x.Id == _redirectId);

                    if (redirect != null)
                    {
                        redirect.Status = 0;
                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0;
                    }

                    return false;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public WhatsappMessageRedirect SaveMessageRedirect(int _messageId, int _departmentId, int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var obj = new WhatsappMessageRedirect()
                    {
                        Date = DateTime.Now,
                        DepartmentId = _departmentId,
                        MessageId = _messageId,
                        Status = 1,
                        RedirectedUserId = _userId
                    };

                    context.WhatsappMessageRedirect.Add(obj);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? obj : null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<WhatsappMessageRedirectRepo> GetRedirectNotifications(int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<WhatsappMessageRedirectRepo>();
                    var headDepartmentIds = context.Department.Where(x => x.HeadOfDepartmentId == _userId).ToList().Select(x => x.Id).ToList();

                    var list = context.WhatsappMessageRedirect.Where(x => headDepartmentIds.Contains(x.DepartmentId) && x.Status > 0).ToList();

                    foreach (var item in list)
                    {
                        var department = context.Department.FirstOrDefault(x => x.Id == item.DepartmentId);
                        var message = context.WhatsappMessage.FirstOrDefault(x => x.Id == item.MessageId);
                        var redirectUser = context.Person.FirstOrDefault(x => x.Id == item.RedirectedUserId);
                        returnList.Add(new WhatsappMessageRedirectRepo()
                        {
                            RedirectDate = item.Date.ToString("dd/MM/yyyy HH:mm"),
                            MessageDate = message.Date.ToString("dd/MM/yyyy HH:mm"),
                            DepartmentId = item.DepartmentId,
                            DepartmentName = department != null ? department.DepartmentName : "Bilinmiyor",
                            Id = item.Id,
                            Message = message != null ? message.MessageContent : "Bilinmiyor",
                            MessageId = item.MessageId,
                            RedirectedUserId = item.RedirectedUserId,
                            RedirectedUserName = redirectUser != null ? $"{redirectUser.Name} {redirectUser.Surname}" : "Bilinmiyor",
                            Status = item.Status,
                            SenderNumber = message != null ? message.MessageFrom : "Bilinmiyor"
                        });
                    }

                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
