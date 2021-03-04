using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class MessageDB
    {
        private static MessageDB instance = null;

        public static MessageDB GetInstance()
        {
            if (instance == null)
                instance = new MessageDB();
            return instance;
        }

        public int GetUnreadMessageCount(int _user)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var count = context.Message.Where(x => x.ReceiverId == _user && x.IsSeen == false).ToList().Count;
                    return count;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void MakeMessagesSeen(int _messageId, int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var message = context.Message.FirstOrDefault(x => x.Id == _messageId);
                    var allMessagesBetween = context.Message.Where(x => x.ReceiverId == message.SenderId && x.SenderId == message.ReceiverId).ToList();
                    allMessagesBetween.AddRange(context.Message.Where(x => x.SenderId == message.SenderId && x.ReceiverId == message.ReceiverId).ToList());
                    allMessagesBetween = allMessagesBetween.Where(x => x.ReceiverId == _userId).ToList();
                    foreach (var item in allMessagesBetween)
                    {
                        item.IsSeen = true;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<MessageRepo> GetMessagesBetweenTwoPeople(int _messageId, int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<MessageRepo>();
                    var message = context.Message.FirstOrDefault(x => x.Id == _messageId);
                    var allMessagesBetween = context.Message.Where(x => x.ReceiverId == message.SenderId && x.SenderId == message.ReceiverId).ToList();
                    allMessagesBetween.AddRange(context.Message.Where(x => x.SenderId == message.SenderId && x.ReceiverId == message.ReceiverId).ToList());
                    allMessagesBetween = allMessagesBetween.OrderBy(x => x.Date).ToList();

                    foreach (var item in allMessagesBetween)
                    {
                        returnList.Add(new MessageRepo()
                        {
                            Date = item.Date.ToString("dd.MM.yyyy - HH:mm"),
                            IsFromMe = item.SenderId == _userId,
                            Message = item.MessageContent,
                            MessageId = item.Id
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

        public Message SendMessage(int _lastMessageId, int _userId, string _text, int receiverId)
        {
            using (var context = new ProjectManagementEntities())
            {
                if (receiverId > 0)
                {
                    var message = new Message()
                    {
                        Date = DateTime.Now,
                        IsSeen = false,
                        MessageContent = _text,
                        ReceiverId = receiverId,
                        SenderId = _userId
                    };

                    context.Message.Add(message);
                    int numberOfUpdated = context.SaveChanges();
                    return message;
                }
                else
                {
                    var lastMessage = context.Message.FirstOrDefault(x => x.Id == _lastMessageId);

                    if (lastMessage != null)
                    {
                        var message = new Message()
                        {
                            Date = DateTime.Now,
                            IsSeen = false,
                            MessageContent = _text,
                            ReceiverId = lastMessage.ReceiverId == _userId ? lastMessage.SenderId : lastMessage.ReceiverId,
                            SenderId = _userId
                        };

                        context.Message.Add(message);
                        int numberOfUpdated = context.SaveChanges();
                        return message;
                    }
                }
                return null;
            }
        }

        public List<MessageRepo> GetMainMessageList(int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<MessageRepo>();
                    var mesajGonderilenUserIdler = context.Message.Where(x => x.SenderId == _userId).Select(x => x.ReceiverId).ToList();
                    var mesajGelenUserIdler = context.Message.Where(x => x.ReceiverId == _userId).Select(x => x.SenderId).ToList();
                    var relationalUserIds = mesajGonderilenUserIdler;
                    relationalUserIds.AddRange(mesajGelenUserIdler);
                    relationalUserIds = relationalUserIds.Distinct().ToList();

                    foreach (var item in relationalUserIds)
                    {
                        var allMessagesBetween = context.Message.Where(x => x.ReceiverId == item && x.SenderId == _userId).ToList();
                        allMessagesBetween.AddRange(context.Message.Where(x => x.SenderId == item && x.ReceiverId == _userId).ToList());
                        allMessagesBetween = allMessagesBetween.OrderByDescending(x => x.Date).ToList();
                        var lastMsg = allMessagesBetween.FirstOrDefault();

                        if (lastMsg != null)
                        {
                            var user = lastMsg.SenderId == _userId ? context.Person.FirstOrDefault(x => x.Id == lastMsg.ReceiverId) : context.Person.FirstOrDefault(x => x.Id == lastMsg.SenderId);

                            returnList.Add(new MessageRepo()
                            {
                                Date = lastMsg.Date.ToString("dd.MM.yyyy - HH:mm"),
                                Message = $"<i style=\"opacity: 0.5; margin-right: 10px;{(lastMsg.ReceiverId == _userId ? " -moz-transform: scale(-1, 1); -webkit-transform: scale(-1, 1); -o-transform: scale(-1, 1); -ms-transform: scale(-1, 1); transform: scale(-1, 1);" : "")}\" class=\"fa fa-share\"></i>" + lastMsg.MessageContent,
                                MessageId = lastMsg.Id,
                                Name = $"{user.Name} {user.Surname}",
                                IsSeen = lastMsg.IsSeen,
                                SenderId = lastMsg.SenderId,
                                ReceiverId = lastMsg.ReceiverId
                            });
                        }
                    }

                    returnList = returnList.OrderByDescending(x => x.MessageId).ToList();

                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw;
            }
        }
    }
}
