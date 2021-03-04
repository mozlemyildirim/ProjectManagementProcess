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
    public class QuickTaskController : BaseController
    {
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult GetAllQuickTask()
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
        public JsonResult GetQuickTaskById(int _taskId)
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
        public List<Person> GetAllQuickTaskUser()
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
        public List<Project> GetAllQuickTaskProject()
        {
            try
            {
                var obj = HttpContext.Session.GetString("SelectedProject");

                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                

                using (var context = new ProjectManagementEntities())
                {

                    if(obj == null)
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
        public JsonResult SaveQuickTask(string _todoName, string _todoEndDate, int _projectId, List<int> _todoUsers)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                if (_projectId == 0)
                {
                    _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                }
                _todoUsers.Add(_personId);

                _todoUsers = _todoUsers.Distinct().ToList();

                var todo = new Task()
                {
                    Detail ="",
                    Title= _todoName,
                    ProjectId = _projectId,
                    EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                    Status = 1,
                    CategoryId=0
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
        public JsonResult UpdateQuickTask(string _todoName, string _todoEndDate, int _todoId, int _projectId, List<int> _todoUsers)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                _todoUsers.Add(_personId);
                _todoUsers = _todoUsers.Distinct().ToList();
                    var QuickTask = new Task()
                    {
                        Title=_todoName,
                        Detail="",
                        EndDate= DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                        Id=_todoId,
                        Status=1,
                        ProjectId=_projectId
                    };
               var result= TaskDB.GetInstance().UpdateTask(QuickTask, _todoUsers);

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteQuickTask(int _todoId)
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