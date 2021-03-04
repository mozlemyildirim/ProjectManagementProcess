using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;
using PMPDAL.Models;
using Task = PMPDAL.Entities.Task;

namespace PMPCore.Controllers
{
    public class DetailTaskController : BaseController
    {
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult GetAllDetailTask()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = TaskDB.GetInstance().GetAllTask().ToList();
                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetTaskById(int _taskId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = TaskDB.GetInstance().GetTaskById(_taskId).ToList();
                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Person> GetAllTaskUser()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = TaskUserDB.GetInstance().GetAllTaskUser();
                    return list;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Project> GetAllDetailTaskProject()
        {
            try
            {
                var obj = HttpContext.Session.GetString("SelectedProject");

                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;


                using (var context = new ProjectManagementEntities())
                {

                    if (obj == null)
                    {
                        var list = ProjectDB.GetInstance().GetAllProject(_personId);
                        return list;
                    }
                    else
                    {
                        var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                        var list = new List<Project>();
                        var item = ProjectDB.GetInstance().GetProjeById(_projectId);
                        list.Add(item);
                        return list;
                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Project> GetAllDetailTaskProjectById(int _id)
        {
            try
            {

                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;


                using (var context = new ProjectManagementEntities())
                {
                    var list = new List<Project>();
                    var item = ProjectDB.GetInstance().GetProjeById(_id);
                    list.Add(item);
                    return list;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetAllDetailTaskByProjectId()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                    var item = TaskDB.GetInstance().GetAllTaskByProjectId2(_projectId).ToList();
                    return item;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                    {
                        var item = TaskDB.GetInstance().GetAllTaskByProjectId2(_projectId).ToList();
                        return item;
                    }
                    else
                    {
                        var returnList = new List<TaskRepo>();
                        var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                        var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                        var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.Status > 0).ToList();
                        foreach (var i in item)
                        {
                            var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                            var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                            returnList.Add(new TaskRepo()
                            {
                                Detail = i.Detail,
                                Title = i.Title,
                                Id = i.Id,
                                EndDate = i.EndDate,
                                Persons = persons,
                                ProjectId = i.ProjectId,
                                Status = i.Status,
                                StrDate = i.EndDate.ToString("dd/MM/yyyy")
                            });
                        }
                        return returnList;
                    }

                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskWithProject()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var obj = HttpContext.Session.GetString("SelectedProject");
                    var _projectIds = context.Project.Where(x => x.Status > 0).Select(x => x.Id).ToList();
                    if (obj == null)
                    {
                        if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                        {
                            var item = TaskDB.GetInstance().GetAllTaskByProjectId(_projectIds).ToList();
                            return item;
                        }
                        else
                        {
                            var returnList = new List<TaskRepo>();
                            var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                            var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                            foreach (var _projectId in _projectIds)
                            {
                                var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.ProjectId == _projectId && x.Status > 0 && x.CategoryId == 0).ToList();
                                foreach (var i in item)
                                {
                                    var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                                    var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                                    returnList.Add(new TaskRepo()
                                    {
                                        Detail = i.Detail,
                                        Title = i.Title,
                                        Id = i.Id,
                                        EndDate = i.EndDate,
                                        Persons = persons,
                                        ProjectId = i.ProjectId,
                                        Status = i.Status,
                                        StrDate = i.EndDate.ToString("dd/MM/yyyy"),
                                        CategoryId = i.CategoryId.ToString() == null ? 0 : i.CategoryId
                                    });
                                }
                                var control = context.Task.Where(x => x.ProjectId == _projectId && x.Status > 0 && x.CategoryId != 0).ToList().Count;
                                if (control > 0)
                                {
                                    var list2 = context.Category.Where(x => x.Status > 0 && _projectId == x.ProjectId).ToList();
                                    foreach (var item2 in list2)
                                    {
                                        returnList.Add(new TaskRepo()
                                        {
                                            Detail = item2.Name,
                                            ProjectId = item2.ProjectId,
                                            CategoryId = item2.Id,
                                            Title = "Category"
                                        });

                                    }
                                }
                            }

                            return returnList;
                        }
                    }
                    else
                    {
                        var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                        if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                        {
                            var item = TaskDB.GetInstance().GetAllTaskByProjectId2(_projectId).ToList();
                            return item;
                        }
                        else
                        {
                            var returnList = new List<TaskRepo>();
                            var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                            var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                            var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.ProjectId == _projectId && x.Status > 0 && x.CategoryId == 0).ToList();
                            foreach (var i in item)
                            {
                                var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                                var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                                returnList.Add(new TaskRepo()
                                {
                                    Detail = i.Detail,
                                    Title = i.Title,
                                    Id = i.Id,
                                    EndDate = i.EndDate,
                                    Persons = persons,
                                    ProjectId = i.ProjectId,
                                    Status = i.Status,
                                    StrDate = i.EndDate.ToString("dd/MM/yyyy"),
                                    CategoryId = i.CategoryId.ToString() == null ? 0 : i.CategoryId
                                });
                            }
                            return returnList;
                        }
                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskWithProjectById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                    {
                        var item = TaskDB.GetInstance().GetAllTaskByProjectId2(_id).ToList();
                        return item;
                    }
                    else
                    {
                        var returnList = new List<TaskRepo>();
                        var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                        var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                        var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.ProjectId == _id && x.Status > 0 && x.CategoryId == 0).ToList();
                        foreach (var i in item)
                        {
                            var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                            var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                            returnList.Add(new TaskRepo()
                            {
                                Detail = i.Detail,
                                Title = i.Title,
                                Id = i.Id,
                                EndDate = i.EndDate,
                                Persons = persons,
                                ProjectId = i.ProjectId,
                                Status = i.Status,
                                StrDate = i.EndDate.ToString("dd/MM/yyyy"),
                                CategoryId = i.CategoryId.ToString() == null ? 0 : i.CategoryId
                            });
                        }
                        return returnList;

                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskWithCategory()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var obj = HttpContext.Session.GetString("SelectedProject");
                    var _projectIds = context.Project.Where(x => x.Status > 0).Select(x => x.Id).ToList();
                    if (obj == null)
                    {
                        if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                        {
                            var item = TaskDB.GetInstance().GetAllTaskCategoryByProjectId(_projectIds).ToList();
                            return item;
                        }
                        else
                        {
                            var returnList = new List<TaskRepo>();
                            var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                            var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                            foreach (var _projectId in _projectIds)
                            {
                                var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.ProjectId == _projectId && x.Status > 0 && x.CategoryId != 0).ToList();
                                foreach (var i in item)
                                {
                                    var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                                    var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                                    returnList.Add(new TaskRepo()
                                    {
                                        Detail = i.Detail,
                                        Title = i.Title,
                                        Id = i.Id,
                                        EndDate = i.EndDate,
                                        Persons = persons,
                                        ProjectId = i.ProjectId,
                                        Status = i.Status,
                                        StrDate = i.EndDate.ToString("dd/MM/yyyy"),
                                        CategoryId = i.CategoryId.ToString() == null ? 0 : i.CategoryId
                                    });
                                }
                            }
                            return returnList;
                        }
                    }
                    else
                    {
                        var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                        if (JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).IsAdmin == 1)
                        {
                            var item = TaskDB.GetInstance().GetAllTaskCategoryByProjectId2(_projectId).ToList();
                            return item;
                        }
                        else
                        {
                            var returnList = new List<TaskRepo>();
                            var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                            var userTasksIds = context.TaskUser.Where(x => x.PersonId == _personId).Select(x => x.TaskId).ToList();
                            var item = context.Task.Where(x => userTasksIds.Contains(x.Id) && x.ProjectId == _projectId && x.Status > 0 && x.CategoryId != 0).ToList();
                            foreach (var i in item)
                            {
                                var personIds = context.TaskUser.Where(x => x.TaskId == i.Id).ToList().Select(x => x.PersonId).ToList();
                                var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                                returnList.Add(new TaskRepo()
                                {
                                    Detail = i.Detail,
                                    Title = i.Title,
                                    Id = i.Id,
                                    EndDate = i.EndDate,
                                    Persons = persons,
                                    ProjectId = i.ProjectId,
                                    Status = i.Status,
                                    StrDate = i.EndDate.ToString("dd/MM/yyyy"),
                                    CategoryId = i.CategoryId.ToString() == null ? 0 : i.CategoryId
                                });
                            }
                            return returnList;
                        }
                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskWithCategoryId(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var item = TaskDB.GetInstance().GetAllTaskCategoryByCatId(_id).ToList();
                    return item;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<TaskRepo> GetDetailTaskWithCategoryByProjectId(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var item = TaskDB.GetInstance().GetAllTaskCategoryByProjectId2(_id).ToList();
                    return item;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult SaveTask(string _todoName, string _todoDetail, string _todoEndDate, int _projectId, List<int> _todoUsers)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                _todoUsers.Add(_personId);

                _todoUsers = _todoUsers.Distinct().ToList();

                var todo = new Task()
                {
                    Title = _todoName,
                    Detail = _todoDetail,
                    ProjectId = _projectId,
                    EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                    Status = 1,
                    CategoryId = 0
                };

                var result = TaskDB.GetInstance().SaveTask(todo);

                foreach (var item in _todoUsers)
                {
                    var todouser = new TaskUser()
                    {
                        TaskId = result.Id,
                        PersonId = item
                    };

                    TaskUserDB.GetInstance().SaveTaskUser(todouser);
                }

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult UpdateTask(string _todoTitle, string _todoDetail, string _todoEndDate, int _todoId, List<int> _todoUsers, List<int> _todoTasks)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                    _todoUsers.Add(_personId);
                    _todoUsers = _todoUsers.Distinct().ToList();
                    var projectId = context.Task.FirstOrDefault(x => x.Id == _todoId).ProjectId;
                    var DetailTask = new Task()
                    {
                        Title = _todoTitle,
                        Detail = _todoDetail,
                        EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                        Id = _todoId,
                        Status = 1,
                        ProjectId = projectId
                    };
                    var result = TaskDB.GetInstance().UpdateTask(DetailTask, _todoUsers);

                    return Json(result != null);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteTask(int _todoId)
        {
            try
            {
                var result = TaskDB.GetInstance().DeleteTask(_todoId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
    }
}