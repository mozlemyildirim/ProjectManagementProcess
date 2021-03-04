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
    public class TaskUserDB
    {
        private TaskUserDB() { }
        private static TaskUserDB instance = null;

        public static TaskUserDB GetInstance()
        {
            if (instance == null)
                instance = new TaskUserDB();
            return instance;
        }
        public List<Person> GetAllTaskUser()
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
        public TaskUser SaveTaskUser(TaskUser _TaskUser)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.TaskUser.Add(_TaskUser);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _TaskUser : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool DeleteTaskUser(int _TaskUserId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var TaskUser = context.TaskUser.FirstOrDefault(x => x.Id == _TaskUserId);

                    if (TaskUser != null)
                    {
                        //TaskUser.Status = 0;
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
