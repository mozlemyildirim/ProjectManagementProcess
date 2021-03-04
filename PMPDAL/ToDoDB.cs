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


namespace PMPDAL
{
    public class ToDoDB
    {
        private static ToDoDB instance = null;

        public static ToDoDB GetInstance()
        {
            if (instance == null)
                instance = new ToDoDB();
            return instance;
        }
        public List<ToDoRepo> GetAllToDo()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<ToDoRepo>();
                    var list = context.ToDo.Where(x => x.Status > 0).ToList();
                    foreach (var item in list)
                    {
                        var personIds = context.ToDoUser.Where(x => x.ToDoId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();

                        returnList.Add(new ToDoRepo()
                        {
                            Detail = item.Detail,
                            Id = item.Id,
                            EndDate = item.EndDate,
                            Persons = persons,
                            ProjectId = item.ProjectId,
                            Status = item.Status,
                            StrDate = item.EndDate.ToString("dd/MM/yyyy")
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
        public List<ToDoRepo> GetToDoById(int _todoId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<ToDoRepo>();
                    var list = context.ToDo.FirstOrDefault(x => x.Status > 0 && x.Id == _todoId);
                        var personIds = context.ToDoUser.Where(x => x.ToDoId == list.Id).ToList().Select(x => x.PersonId).ToList();
                        var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                        returnList.Add(new ToDoRepo()
                        {
                            Detail = list.Detail,
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
        public ToDo SaveToDo(ToDo _todo)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.ToDo.Add(_todo);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _todo : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public ToDo UpdateToDo(ToDo _s, List<int> _workerIds)
        {
            try
            {
                var numberOfUpdated = 0;
                using (var context = new ProjectManagementEntities())
                {
                    var todo = context.ToDo.FirstOrDefault(x => x.Id == _s.Id);

                    if (todo != null)
                    {
                        todo.Detail = _s.Detail;
                        todo.EndDate = _s.EndDate;
                        numberOfUpdated = context.SaveChanges();
                        var todoPerson = context.ToDoUser.Where(x => x.ToDoId == _s.Id).ToList();

                        if (todoPerson.Count() > 0)
                        {
                            foreach (var todoPersonItem in todoPerson)
                            {
                                var todoPersonDetail = context.ToDoUser.Where(x => x.PersonId == todoPersonItem.PersonId).ToList();
                                context.ToDoUser.RemoveRange(todoPersonDetail);
                                context.SaveChanges();
                            }
                        }
                        foreach (var item in _workerIds)
                        {

                            var newToDoPerson = new ToDoUser()
                            {
                                PersonId = item,
                                ToDoId = _s.Id
                            };

                            context.ToDoUser.Add(newToDoPerson);
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
        public bool DeleteToDo(int _todoId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var todo = context.ToDo.FirstOrDefault(x => x.Id == _todoId);

                    if (todo != null)
                    {
                        todo.Status = 0;
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

    public class ToDoRepo : ToDo
    {
        public List<Person> Persons { get; set; }
        public string StrDate { get; set; }
    }
}

