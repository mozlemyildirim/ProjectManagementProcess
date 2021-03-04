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
    public class ToDoUserDB
    {
        private ToDoUserDB() { }
        private static ToDoUserDB instance = null;

        public static ToDoUserDB GetInstance()
        {
            if (instance == null)
                instance = new ToDoUserDB();
            return instance;
        }
        public List<Person> GetAllToDoUser()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.Person.Where(x => x.Status > 0).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public ToDoUser SaveToDoUser(ToDoUser _toDoUser)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.ToDoUser.Add(_toDoUser);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _toDoUser : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool DeleteToDoUser(int _ToDoUserId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var ToDoUser = context.ToDoUser.FirstOrDefault(x => x.Id == _ToDoUserId);

                    if (ToDoUser != null)
                    {
                        //ToDoUser.Status = 0;
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
}
