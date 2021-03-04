using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class TeamDB
    {
        private static TeamDB instance = null;

        public static TeamDB GetInstance()
        {
            if (instance == null)
                instance = new TeamDB();
            return instance;
        }

        public List<Team> GetAllTeam()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.Team.Where(x => x.Status == 1).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<TeamRepo> GetAllTeamSelect()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<TeamRepo>();
                    var list = context.Team.Where(x => x.Status == 1).ToList();

                    foreach (var item in list)
                    {
                        var department = context.Department.FirstOrDefault(x => x.Id == item.DepartmentId);

                        returnList.Add(new TeamRepo()
                        {
                            DepartmentName = department != null ? department.DepartmentName : "Bulunamadı...",
                            Id = item.Id,
                            Status = item.Status,
                            Name = item.Name
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


        public Team SaveTeam(int _id, string _name, int _departmentId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (_id == 0)
                    {
                        var Team = new Team()
                        {
                            Id = _id,
                            Name = _name,
                            Status = 1,
                            DepartmentId = _departmentId
                        };

                        context.Team.Add(Team);

                        int numberOfInserted = context.SaveChanges();

                        return numberOfInserted > 0 ? Team : null;
                    }
                    else
                    {
                        var Team = context.Team.FirstOrDefault(x => x.Id == _id);
                        Team.Id = _id;
                        Team.Name = _name;
                        Team.Status = 1;
                        Team.DepartmentId = _departmentId;

                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0 ? Team : null;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        public Team GetTeamById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var Team = context.Team.FirstOrDefault(x => x.Id == _id);
                    return Team;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }




        public bool DeleteTeam(int _teamId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var team = context.Team.FirstOrDefault(x => x.Id == _teamId);
                    team.Status = 0;
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
