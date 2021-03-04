using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = PMPDAL.Entities.Task;

namespace PMPDAL
{
    public class TaskDB
    {
        private static TaskDB instance = null;

        public static TaskDB GetInstance()
        {
            if (instance == null)
                instance = new TaskDB();
            return instance;
        }
        public List<TaskRepo> GetAllTask()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title=item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy")
                            
                        });
                    }
                    return returnList.OrderByDescending(x => x.EndDate).ToList(); ;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetAllTaskByProjectId(List<int> _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0 && _id.Contains(x.ProjectId) && x.CategoryId==0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title = item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy"),
                            CategoryId = item.CategoryId.ToString() == null ? 0 : item.CategoryId
                        });
                        returnList.OrderByDescending(x => x.EndDate).ToList();
                    }
                    var control = context.Task.Where(x => x.Status > 0 && _id.Contains(x.ProjectId) && x.CategoryId != 0).ToList().Count;
                    if (control > 0)
                    {
                        var list2 = context.Category.Where(x => x.Status > 0 && _id.Contains(x.ProjectId)).ToList();
                        foreach (var item in list2)
                        {
                            returnList.Add(new TaskRepo()
                            {
                                Detail = item.Name,
                                ProjectId = item.ProjectId,
                                CategoryId = item.Id,
                                Title="Category"
                            });

                        }
                    }
                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetAllTaskCategoryByProjectId(List<int> _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0 && _id.Contains(x.ProjectId) && x.CategoryId != 0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title = item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy"),
                            CategoryId = item.CategoryId.ToString() == null ? 0 : item.CategoryId
                        });
                    }
                    return returnList.OrderByDescending(x => x.EndDate).ToList();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetAllTaskCategoryByProjectId2(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0 && x.ProjectId == _id && x.CategoryId != 0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title = item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy"),
                            CategoryId = item.CategoryId
                        });
                    }
                    return returnList.OrderByDescending(x=>x.EndDate).ToList();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetAllTaskCategoryByCatId(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0 && x.CategoryId == _id).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title = item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy"),
                            CategoryId = item.CategoryId
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
        public List<TaskRepo> GetAllTaskByProjectId2(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.Where(x => x.Status > 0 && x.ProjectId==_id && x.CategoryId==0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.TaskUser.Where(x => x.TaskId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new TaskRepo()
                        {
                            Detail = item.Detail,
                            Title = item.Title,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy"),
                            CategoryId=item.CategoryId
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
        public List<TaskRepo> GetTaskById(int _TaskId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TaskRepo>();
                    var list = context.Task.FirstOrDefault(x => x.Status > 0 && x.Id == _TaskId);
                        var personIds = context.TaskUser.Where(x => x.TaskId == list.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                        returnList.Add(new TaskRepo()
                        {
                            Detail = list.Detail,
                            Title=list.Title,
                            Id = list.Id,
                            EndDate = list.EndDate,
                            Persons = persons,
                            ProjectId = list.ProjectId,
                            Status = list.Status,
                            StrDate = list.EndDate.ToString("dd/MM/yyyy")
                        });
                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public Task SaveTask(Task _Task)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Task.Add(_Task);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _Task : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Task UpdateTask(Task _s, List<int> _workerIds)
        {
            try
            {
                var numberOfUpdated = 0;
                using (var context = new ProjectManagementEntities())
                {
                    var Task = context.Task.FirstOrDefault(x => x.Id == _s.Id);

                    if (Task != null)
                    {
                        Task.Title = _s.Title;
                        Task.Detail = _s.Detail;
                        Task.EndDate = _s.EndDate;
                        numberOfUpdated = context.SaveChanges();
                        var TaskPerson = context.TaskUser.Where(x => x.TaskId == _s.Id).ToList();

                        if (TaskPerson.Count() > 0)
                        {
                            foreach (var TaskPersonItem in TaskPerson)
                            {
                                var TaskPersonDetail = context.TaskUser.Where(x => x.PersonId == TaskPersonItem.PersonId).ToList();
                                context.TaskUser.RemoveRange(TaskPersonDetail);
                                context.SaveChanges();
                            }
                        }
                        foreach (var item in _workerIds)
                        {

                            var newTaskPerson = new TaskUser()
                            {
                                PersonId = item,
                                TaskId = _s.Id
                            };

                            context.TaskUser.Add(newTaskPerson);
                            context.SaveChanges();
                        }
                        return numberOfUpdated > 0 ? _s : null;
                    }
                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool DeleteTask(int _TaskId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var Task = context.Task.FirstOrDefault(x => x.Id == _TaskId);

                    if (Task != null)
                    {
                        Task.Status = 0;
                        int numberOfDeleted = context.SaveChanges();

                        return numberOfDeleted > 0;
                    }

                }
                return false;

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }
    }

    public class TaskRepo : Task
    {
        public List<Person> Persons { get; set; }
        public string StrDate { get; set; }
    }
}

