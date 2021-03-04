using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class ProjectDB
    {
        private static ProjectDB instance = null;

        public static ProjectDB GetInstance()
        {
            if (instance == null)
                instance = new ProjectDB();
            return instance;
        }

        public List<Project> GetAllProject(int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<Project>();
                    var person = context.Person.FirstOrDefault(x => x.Id == _personId);

                    //if (person.IsAdmin == 1)
                    //{
                    //    returnList = context.Project.Where(x => x.Status == 1).ToList();
                    //}
                    //else
                    //{
                    var projectIds = context.ProjectPerson.Where(x => x.PersonId == _personId).ToList().Select(x => x.ProjectId).ToList();
                    returnList = context.Project.Where(x => x.Status == 1 && projectIds.Contains(x.Id)).ToList();
                    //}

                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public Project SaveProject(Project _s)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Project.Add(_s);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _s : null;

                }
            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public object GetProjeById(object projectId)
        {
            throw new NotImplementedException();
        }

        public Project UpdateProject(Project _s)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var project = context.Project.FirstOrDefault(x => x.Id == _s.Id);

                    if (project != null)
                    {
                        project.Description = _s.Description;
                        project.Code = _s.Code;
                        project.ShortName = _s.ShortName;
                        project.Name = _s.Name;
                        project.ProjectLeader = _s.ProjectLeader;
                        project.Status = _s.Status;

                        int numberOfUpdated = context.SaveChanges();

                        return numberOfUpdated > 0 ? project : null;
                    }
                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }



        public bool DeleteProje(int _projeId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var proje = context.Project.FirstOrDefault(x => x.Id == _projeId);

                    if (proje != null)
                    {
                        proje.Status = 0;
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


        public Project GetProjeById(int _projeId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var proje = context.Project.FirstOrDefault(x => x.Id == _projeId);

                    if (proje != null)
                    {
                        var returnModel = new Project()
                        {
                            Description = proje.Description == null ? "" : proje.Description,
                            Id = proje.Id,
                            Name = proje.Name == null ? "" : proje.Name,
                            Code = proje.Code,
                            ProjectLeader = proje.ProjectLeader,
                            ShortName = proje.ShortName

                        };
                        return returnModel;
                    }


                }
                return null;

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public ProjectPerson SaveProjectPerson(ProjectPerson _pp)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.ProjectPerson.Add(_pp);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _pp : null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<ProjectPerson> GetAllProjectPersonsByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.ProjectPerson.Where(x => x.ProjectId == _projectId).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public bool DeleteProjectPersonByProjectId(int _projeId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var projectPersons = context.ProjectPerson.Where(x => x.ProjectId == _projeId).ToList();

                    context.ProjectPerson.RemoveRange(projectPersons);
                    int numberOfDeleted = context.SaveChanges();
                    return numberOfDeleted > 0;
                }

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }
    }
}
