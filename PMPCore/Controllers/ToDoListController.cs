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
    public class ToDoListController : BaseController
    {
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult SaveKategori(string _name)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;


                var category = new Category()
                {
                    Name = _name,
                    ProjectId = _projectId,
                    Status = 1
                };

                var result = CategoryDB.GetInstance().SaveCategory(category);

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult UpdateKategori(string _name,int _id)
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;

                var category = new Category()
                {
                    Id=_id,
                    Name = _name,
                    ProjectId = _projectId,
                    Status = 1
                };

                var result = CategoryDB.GetInstance().UpdateCategory(category);

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
            
        }
        public JsonResult GetCategories()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var obj = HttpContext.Session.GetString("SelectedProject");
                    var list = new List<Category>();
                    if (obj == null)
                    {
                        list = CategoryDB.GetInstance().GetAllCategory().ToList();
                    }
                    else
                    {
                        var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                        list = CategoryDB.GetInstance().GetAllCategoryByProjectId(_projectId).ToList();
                    }
                    
                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetCategoriesById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var obj = HttpContext.Session.GetString("SelectedProject");
                    var list = new List<Category>();
                        list = CategoryDB.GetInstance().GetCategoryById(_id).ToList();

                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetCategoriesByProjectId(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = new List<Category>();
                    list = CategoryDB.GetInstance().GetAllCategoryByProjectId(_id).ToList();

                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public List<Category> GetCategoryNameandId(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<Category>();
                    var list = context.Category.FirstOrDefault(x => x.Id == _id);
                    returnList.Add(new Category()
                    {
                        Id = list.Id,
                        Name = list.Name,
                        ProjectId = list.ProjectId
                    });
                    return returnList;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetAllToDoList()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = ToDoDB.GetInstance().GetAllToDo().ToList();
                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetToDoById(int _todoId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = ToDoDB.GetInstance().GetToDoById(_todoId).ToList();
                    return Json(list);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Person> GetAllToDoUser()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = ToDoUserDB.GetInstance().GetAllToDoUser();
                    return list;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Project> GetAllToDoProject()
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("ActivePerson")).Id;
                using (var context = new ProjectManagementEntities())
                {
                    var list = ProjectDB.GetInstance().GetAllProject(_personId);
                    return list;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult SaveToDo(string _todoName, string _todoEndDate, int _projectId, List<int> _todoUsers)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("ActivePerson")).Id;
                _todoUsers.Add(_personId);

                _todoUsers = _todoUsers.Distinct().ToList();

                var todo = new ToDo()
                {
                    Detail = _todoName,
                    ProjectId = _projectId,
                    EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                    Status = 1
                };

                var result = ToDoDB.GetInstance().SaveToDo(todo);

                foreach (var item in _todoUsers)
                {
                    var todouser = new ToDoUser()
                    {
                        ToDoId = result.Id,
                        PersonId = item
                    };

                    ToDoUserDB.GetInstance().SaveToDoUser(todouser);
                }

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult SaveCategoryStep(string _todoName, string _todoEndDate, int _projectId,int _catId, List<int> _todoUsers)
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
                    Detail = "",
                    Title = _todoName,
                    ProjectId = _projectId,
                    EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                    Status = 1,
                    CategoryId = _catId
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
        public JsonResult UpdateToDo(string _todoName, string _todoEndDate, int _todoId, int _projectId, List<int> _todoUsers)
        {
            try
            {
                var _personId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("ActivePerson")).Id;
                _todoUsers.Add(_personId);
                _todoUsers = _todoUsers.Distinct().ToList();
                var todolist = new ToDo()
                {
                    Detail = _todoName,
                    EndDate = DateTime.ParseExact(_todoEndDate, "dd/MM/yyyy", null),
                    Id = _todoId,
                    Status = 1,
                    ProjectId = _projectId
                };
                var result = ToDoDB.GetInstance().UpdateToDo(todolist, _todoUsers);

                return Json(result != null);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult DeleteToDo(int _todoId)
        {
            try
            {
                var result = ToDoDB.GetInstance().DeleteToDo(_todoId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult DeleteCategory(int _id)
        {
            try
            {
                var result = CategoryDB.GetInstance().DeleteCategory(_id);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
    }
}