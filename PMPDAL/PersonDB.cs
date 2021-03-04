using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class PersonDB
    {
        private static PersonDB instance = null;

        public static PersonDB GetInstance()
        {
            if (instance == null)
                instance = new PersonDB();
            return instance;
        }

        public Person ControlPerson(string _email, string _password)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var user = context.Person.FirstOrDefault(x => x.Email == _email && x.Password == _password && x.Status > 0);

                    if (user != null)
                        return user;
                    return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Person GetPersonById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var person = context.Person.FirstOrDefault(x => x.Id == _id);
                    return person;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<PersonViewModel> GetAllPerson()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<PersonViewModel>();
                    var list = context.Person.Where(x => x.Status == 1).ToList();

                    foreach (var item in list)
                    {
                        returnList.Add(new PersonViewModel()
                        {
                            Email = item.Email,
                            Id = item.Id,
                            Name = item.Name,
                            Password = item.Password,
                            Status = item.Status,
                            Surname = item.Surname,
                            TeamId = item.TeamId,
                            TeamName = context.Team.FirstOrDefault(x => x.Id == item.TeamId).Name,
                            IsAdmin = item.IsAdmin
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

        public List<Person> GetHeadOfDepartmentsUsers(int _userId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var departmentIds = context.Department.Where(x => x.HeadOfDepartmentId == _userId && x.Status == 1).ToList().Select(x => x.Id).ToList();
                    var teamIds = context.Team.Where(x => departmentIds.Contains(x.DepartmentId) && x.Status == 1).ToList().Select(x => x.Id).ToList();
                    var persons = context.Person.Where(x => teamIds.Contains(x.TeamId) && x.Status == 1).ToList();
                    return persons;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Person SavePerson(int _id, string _name, string _surname, string _mail, string _password, int _teamId, bool _isAdmin)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (_id == 0)
                    {
                        var person = new Person()
                        {
                            Email = _mail,
                            Name = _name,
                            Password = _password,
                            Status = 1,
                            Surname = _surname,
                            TeamId = _teamId,
                            IsAdmin = _isAdmin ? 1 : 0
                        };

                        context.Person.Add(person);

                        int numberOfInserted = context.SaveChanges();

                        return numberOfInserted > 0 ? person : null;
                    }
                    else
                    {
                        var person = context.Person.FirstOrDefault(x => x.Id == _id);
                        person.Email = _mail;
                        person.Name = _name;
                        person.Password = _password;
                        person.Status = 1;
                        person.Surname = _surname;
                        person.TeamId = _teamId;
                        person.IsAdmin = _isAdmin ? 1 : 0;
                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0 ? person : null;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeletePerson(int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var person = context.Person.FirstOrDefault(x => x.Id == _personId);
                    person.Status = 0;
                    int numberOfUpdated = context.SaveChanges();
                    return numberOfUpdated > 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
