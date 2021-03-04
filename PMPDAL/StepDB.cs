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
    public class StepDB
    {
        private static StepDB instance = null;

        public static StepDB GetInstance()
        {
            if (instance == null)
                instance = new StepDB();
            return instance;
        }

        public List<Step> GetAllStep(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprints = context.Sprint.Where(x => x.ProjectId == _projectId).ToList();
                    var sprintIds = sprints.Select(x => x.Id).ToList();
                    var list = context.Step.Where(x => sprintIds.Contains(x.SprintId) && x.Status > 0).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Step SaveWhatsappTaskAsStep(int _redirectId, int _milestoneId, int _sprintId, int _headOfDepartmentPersonId, int _personId, string _title, string _desc)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var redirect = context.WhatsappMessageRedirect.FirstOrDefault(x => x.Id == _redirectId);
                    var message = context.WhatsappMessage.FirstOrDefault(x => x.Id == redirect.MessageId);

                    var step = new Step()
                    {
                        AssigneeUser = _headOfDepartmentPersonId,
                        Description = _desc,
                        MilestoneId = _milestoneId,
                        Name = _title,
                        SprintId = _sprintId,
                        Status = 1
                    };

                    context.Step.Add(step);
                    int numberOfInserted = context.SaveChanges();

                    if (numberOfInserted > 0)
                    {
                        var stepPerson = new StepPerson()
                        {
                            Description = "",
                            PersonId = _personId,
                            StepId = step.Id
                        };
                        context.StepPerson.Add(stepPerson);
                        context.SaveChanges();

                        var sprint = context.Sprint.FirstOrDefault(x => x.Id == _sprintId);

                        if (sprint.Status == 2)
                        {
                            var boardStep = new BoardStep()
                            {
                                Date = DateTime.Now,
                                Description = "Whatsapp üzerinden yönlendirilmiştir.",
                                PersonId = _headOfDepartmentPersonId,
                                BoardId = context.Board.Where(x => x.ProjectId == sprint.ProjectId && x.Status > 0).ToList().FirstOrDefault().Id,
                                StepId = step.Id
                            };

                            context.BoardStep.Add(boardStep);
                            context.SaveChanges();
                        }

                        redirect.Status = 0;
                        context.SaveChanges();

                        return step;
                    }

                    return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<string> GetStepPersonsByStepId(int _stepId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var nameSurnameList = new List<string>();
                    var stepPersons = context.StepPerson.Where(x => x.StepId == _stepId).ToList();

                    foreach (var item in stepPersons)
                    {
                        var person = context.Person.FirstOrDefault(x => x.Id == item.PersonId);

                        nameSurnameList.Add($"{person.Name} {person.Surname}");
                    }

                    return nameSurnameList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Step SaveStep(Step _s, List<int> _workerIds)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Step.Add(_s);
                    int numberOfInserted = context.SaveChanges();

                    if (numberOfInserted > 0)
                    {

                        foreach (var item in _workerIds)
                        {
                            var stepPerson = new StepPerson()
                            {
                                PersonId = item,
                                StepId = _s.Id
                            };

                            context.StepPerson.Add(stepPerson);
                            context.SaveChanges();
                        }

                        var sprint = context.Sprint.FirstOrDefault(x => x.Id == _s.SprintId);

                        if (sprint.Status == 2)
                        {
                            var boardstep = new BoardStep()
                            {
                                BoardId = context.Board.Where(x => x.ProjectId == sprint.ProjectId).ToList().OrderBy(x => x.Id).ToList().FirstOrDefault().Id,
                                Date = DateTime.Now,
                                Description = "",
                                PersonId = 0,
                                StepId = _s.Id
                            };
                            context.BoardStep.Add(boardstep);
                            context.SaveChanges();

                            //foreach (var item in _workerIds)
                            //{
                            //    var boardstep = new BoardStep()
                            //    {
                            //        BoardId = context.Board.Where(x => x.ProjectId == sprint.ProjectId).ToList().OrderBy(x => x.Id).ToList().FirstOrDefault().Id,
                            //        Date = DateTime.Now,
                            //        Description = "",
                            //        PersonId = item,
                            //        StepId = _s.Id
                            //    };
                            //    context.BoardStep.Add(boardstep);
                            //    context.SaveChanges();
                            //}
                        }
                    }
                    return numberOfInserted > 0 ? _s : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Step SaveStep2(Step _s)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Step.Add(_s);
                    int numberOfInserted = context.SaveChanges();

                    if (numberOfInserted > 0)
                    {
                        var step = new Step()
                        {
                            Name = _s.Name,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(7),
                            Description = _s.Name,
                            AssigneeUser = _s.AssigneeUser,
                            Status = 1
                        };
                        context.Step.Add(step);
                        context.SaveChanges();
                    }
                    return numberOfInserted > 0 ? _s : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Step UpdateStep(Step _s, List<int> _workerIds)
        {
            try
            {
                var numberOfUpdated = 0;
                using (var context = new ProjectManagementEntities())
                {
                    var step = context.Step.FirstOrDefault(x => x.Id == _s.Id);

                    if (step != null)
                    {
                        step.AssigneeUser = _s.AssigneeUser;
                        step.Description = _s.Description;
                        step.EndDate = _s.EndDate;
                        step.MilestoneId = _s.MilestoneId;
                        step.Name = _s.Name;
                        step.SprintId = _s.SprintId;
                        step.StartDate = _s.StartDate;

                        numberOfUpdated = context.SaveChanges();

                        var stepPerson = context.StepPerson.Where(x => x.StepId == _s.Id).ToList();

                        if (stepPerson.Count() > 0)
                        {
                            foreach (var stepPersonItem in stepPerson)
                            {
                                var stepPersonDetail = context.StepPersonDetail.Where(x => x.StepPersonId == stepPersonItem.Id).ToList();

                                if (stepPersonDetail.Count() > 0)
                                {
                                    foreach (var stepPersonDetailItem in stepPersonDetail)
                                    {
                                        context.Entry(stepPersonDetailItem).State = EntityState.Deleted;
                                        context.SaveChanges();
                                    }
                                }

                                context.Entry(stepPersonItem).State = EntityState.Deleted;
                                context.SaveChanges();
                            }
                        }

                        foreach (var item in _workerIds)
                        {

                            var newStepPerson = new StepPerson()
                            {
                                PersonId = item,
                                StepId = _s.Id
                            };

                            context.StepPerson.Add(newStepPerson);
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

        public bool DeleteGorev(int _gorevId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var gorev = context.Step.FirstOrDefault(x => x.Id == _gorevId);

                    if (gorev != null)
                    {
                        gorev.Status = 0;
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

        public List<Milestone> GetAllMilestone(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var milestone = context.Milestone.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList();
                    return milestone;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public List<Notification> GetAllNotification(int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list1 = new List<Notification>();
                    var now = DateTime.Now;
                    var person = context.Person.FirstOrDefault(x => x.Id == _personId);
                    if (person.IsAdmin != 0)
                    {
                        var stepProjectIds = context.Project.Where(x => x.Status != 0).Select(x => x.Id);
                        var stepMilestoneIds = context.Milestone.Where(x => stepProjectIds.Contains(x.ProjectId)).Select(x => x.Id);
                        var notifications = context.Step.Where(x => x.Status !=3 && x.Status!=0 && stepMilestoneIds.Contains(x.MilestoneId)).ToList();
                        foreach (var item in notifications)
                        {
                            var kalan = (now.Day) - (Convert.ToDateTime(item.EndDate).Day);
                            if (kalan < 3 && kalan >= 0)
                            {
                                var projectId = context.Milestone.FirstOrDefault(x => x.Id == item.MilestoneId).ProjectId;
                                var projectName = context.Project.FirstOrDefault(x => x.Id == projectId).Name;
                                list1.Add(new Notification()
                                {
                                    Id = item.Id,
                                    AssigneeUser = item.AssigneeUser,
                                    Description = item.Description,
                                    EndDate = item.EndDate,
                                    MilestoneId = item.MilestoneId,
                                    Name = item.Name,
                                    SprintId = item.SprintId,
                                    StartDate = item.StartDate,
                                    Status = item.Status,
                                    Kalan = kalan,
                                    ProjectName = projectName,
                                    ProjectId = projectId
                                });
                            }
                        }
                        return list1;
                    }
                    else
                    {
                        var stepIds = context.StepPerson.Where(x => x.PersonId == _personId).Select(x => x.StepId);
                        var notifications = context.Step.Where(x => x.Status != 3 && x.Status != 0 && stepIds.Contains(x.Id)).ToList();
                        foreach (var item in notifications)
                        {
                            var kalan = (now.Day) - (Convert.ToDateTime(item.EndDate).Day);
                            if (kalan < 3 && kalan >= 0)
                            {
                                var projectId = context.Milestone.FirstOrDefault(x => x.Id == item.MilestoneId).ProjectId;
                                var projectName = context.Project.FirstOrDefault(x => x.Id == projectId).Name;
                                list1.Add(new Notification()
                                {
                                    Id = item.Id,
                                    AssigneeUser = item.AssigneeUser,
                                    Description = item.Description,
                                    EndDate = item.EndDate,
                                    MilestoneId = item.MilestoneId,
                                    Name = item.Name,
                                    SprintId = item.SprintId,
                                    StartDate = item.StartDate,
                                    Status = item.Status,
                                    Kalan = kalan,
                                    ProjectName = projectName,
                                    ProjectId = projectId
                                });
                            }
                        }
                        return list1;
                    }
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public int GetAllNotificationByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list1 = new List<Notification>();
                    var now = DateTime.Now;
                    var milestone = context.Milestone.Where(x => x.ProjectId == _projectId).Select(x => x.Id).ToList();
                    var notifications = context.Step.Where(x => milestone.Contains(x.MilestoneId) && x.Status!=0 && x.Status!=3).ToList();
                    var counter = 0;
                    foreach (var item in notifications)
                    {
                        var kalan = (now.Day) - (Convert.ToDateTime(item.EndDate).Day);
                        if (kalan < 3 && kalan >= 0)
                        {
                            counter ++;
                        }
                    }
                    return counter;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public GetPercentage GetProjectPercentage(int _projectId)
        {
            using (var context = new ProjectManagementEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = $@"SET @bq = 0; 
                                                 SET @pct = 0; 
                                                 SET @boardId = 0;
                                                 SET @pct = 0;
                                                 SET @allItems = 0;
                                                 SET @completedItems = 0;
                                                 SET @aqwp = 0;
                                                 SET @bqwp = 0;
                                                 
                                                 SET @boardId = (SELECT Id FROM Board AS B 
                                                 WHERE ProjectId = {_projectId}
                                                 LIMIT 2,1);
                                                 
                                                 SET @allItems = (SELECT COUNT(1) FROM Step AS S INNER JOIN Milestone AS MS ON MS.Id = S.MilestoneId WHERE MS.ProjectId = {_projectId});
                                                 SET @completedItems = (SELECT COUNT(1) FROM Step AS S INNER JOIN Milestone AS MS ON MS.Id = S.MilestoneId INNER JOIN BoardStep AS BS ON BS.StepId = S.Id  WHERE MS.ProjectId = {_projectId} AND BS.BoardId = @boardId);
                                                 SET @pct = (@completedItems * 100 / @allItems) / 100;
                                                 
                                                 SET @bq = (SELECT Sum(( Datediff(S.enddate, S.startdate) * 24 ) / 3) 
                                                               FROM   Step AS S 
                                                                      INNER JOIN Milestone AS M 
                                                                              ON M.id = S.Milestoneid 
                                                                      INNER JOIN Project AS P 
                                                                              ON P.id = M.Projectid 
                                                               WHERE  P.id = {_projectId});
                                                 
                                                 SET @aqwp = (SELECT Sum(( Datediff(S.enddate, S.startdate) * 24 ) / 3) 
                                                               FROM   Step AS S 
                                                                      INNER JOIN Milestone AS M 
                                                                              ON M.id = S.Milestoneid 
                                                                      INNER JOIN Project AS P 
                                                                              ON P.id = M.Projectid 
					                                                  INNER JOIN BoardStep AS BS ON BS.StepId = S.Id
                                                               WHERE P.id = {_projectId}
                                                                 AND BS.BoardId = @boardId);
                                                 
                                                 SET @bqwp = @pct*@bq;
                                                 
                                                 SELECT @bqwp AS Butce, @aqwp AS Ilerleme, @pct AS Gerceklesen, @bq AS Kazanilan;";

                            using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader())
                            {
                                var tb = new DataTable();
                                tb.Load(dr);
                                var json = JsonConvert.SerializeObject(tb);

                                var result = JsonConvert.DeserializeObject<List<GetPercentage>>(json).FirstOrDefault();

                                return result;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public List<Person> GetAllKullanici()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var kullanici = context.Person.Where(x => x.Status > 0).ToList();
                    return kullanici;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public List<Sprint> GetAllSprint(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprintList = context.Sprint.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList();
                    return sprintList;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public Step GetGorevById(int _gorevId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var gorev = context.Step.FirstOrDefault(x => x.Id == _gorevId);
                    if (gorev != null)
                    {
                        var returnModel = new Step()
                        {
                            AssigneeUser = gorev.AssigneeUser,
                            Description = gorev.Description,
                            EndDate = gorev.EndDate,
                            Id = gorev.Id,
                            MilestoneId = gorev.MilestoneId,
                            Name = gorev.Name,
                            SprintId = gorev.SprintId,
                            StartDate = gorev.StartDate
                        };
                        return returnModel;
                    }
                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public List<Step> GetGorevBySprintId(int _sprintId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var gorev = context.Step.Where(x => x.SprintId == _sprintId).ToList();

                    return gorev;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public List<Person> GetStepPersonListByStepId(int _stepId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var personIds = context.StepPerson.Where(x => x.StepId == _stepId).ToList().Select(x => x.PersonId).ToList();
                    var persons = context.Person.Where(x => personIds.Contains(x.Id)).ToList();
                    return persons;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<StartStopViewModel> GetStepPersons(int _personId, int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<StartStopViewModel>();
                    var stepPersons = context.StepPerson.Where(x => x.PersonId == _personId).ToList();

                    foreach (var item in stepPersons)
                    {
                        var step = context.Step.FirstOrDefault(x => x.Id == item.StepId && x.Status == 2);

                        if (step == null)
                            continue;

                        var milestone = context.Milestone.FirstOrDefault(x => x.Id == step.MilestoneId && x.ProjectId == _projectId);

                        if (milestone == null)
                            continue;

                        var stepPersonDetailList = context.StepPersonDetail.Where(x => x.StepPersonId == item.Id).ToList().OrderBy(x => x.Id).ToList();
                        var lastStepPersonDetail = stepPersonDetailList.LastOrDefault();

                        returnList.Add(new StartStopViewModel()
                        {
                            Description = item.Description,
                            EndDate = lastStepPersonDetail == null ? null : lastStepPersonDetail.EndDate,
                            Id = item.Id,
                            MilestoneName = milestone.Name,
                            PersonId = item.PersonId,
                            StartDate = lastStepPersonDetail == null ? null : (Nullable<DateTime>)lastStepPersonDetail.StartDate,
                            StepId = item.StepId,
                            StepName = step.Name
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

        public StepPersonDetail SaveStepPersonStatus(int _stepPersonId, string _status, string _desc)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (_status == "play")
                    {
                        var stepPersonDetail = new StepPersonDetail()
                        {
                            Description = "Adıma devam ediliyor.",
                            EndDate = null,
                            StartDate = DateTime.Now,
                            StepPersonId = _stepPersonId
                        };

                        context.StepPersonDetail.Add(stepPersonDetail);
                        int numberOfInserted = context.SaveChanges();
                        return numberOfInserted == 0 ? null : stepPersonDetail;
                    }
                    else
                    {
                        var lastRecord = context.StepPersonDetail.FirstOrDefault(x => x.StepPersonId == _stepPersonId && x.EndDate == null);
                        lastRecord.EndDate = DateTime.Now;
                        lastRecord.Description = _desc;
                        int numberOfUpdated = context.SaveChanges();
                        return numberOfUpdated > 0 ? lastRecord : null;
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int GetMillisecondsBetweenTwoDate(DateTime dt1, DateTime dt2)
        {
            TimeSpan span = dt2 - dt1;
            int ms = (int)span.TotalMilliseconds;
            return ms;
        }

        public string MilliSecondsToHumanReadableDateTime(int _milliSeconds)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(_milliSeconds);
            string answer = string.Format("{0:D2} saat {1:D2} dakika",
                                    t.Hours,
                                    t.Minutes);
            return answer;
        }

        public string GetWorkersForChartDetail(int _stepId)
        {
            try
            {
                var list = new List<string>();
                using (var context = new ProjectManagementEntities())
                {
                    var stepPersons = context.StepPerson.Where(x => x.StepId == _stepId).ToList();

                    foreach (var item in stepPersons)
                    {

                        var person = context.Person.FirstOrDefault(x => x.Id == item.PersonId);
                        var stepPersonDetails = context.StepPersonDetail.Where(x => x.StepPersonId == item.Id && x.EndDate != null).ToList();
                        int milliSeconds = 0;

                        stepPersonDetails.ForEach(item2 =>
                        {
                            milliSeconds += GetMillisecondsBetweenTwoDate(item2.StartDate, (DateTime)item2.EndDate);
                        });

                        list.Add($"{person.Name} {person.Surname} ({MilliSecondsToHumanReadableDateTime(milliSeconds)} çalıştı{(context.StepPersonDetail.FirstOrDefault(x => x.StepPersonId == item.Id && x.EndDate == null) != null ? " ve çalışmaya devam ediyor" : "")})");


                    }
                }

                return string.Join(", ", list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<ChartDetailViewModel> GetChartDetail(string _milestoneName, int _projectId)
        {
            try
            {
                var returnList = new List<ChartDetailViewModel>();
                using (var context = new ProjectManagementEntities())
                {
                    var milestone = context.Milestone.FirstOrDefault(x => x.Name == _milestoneName.Trim() && x.ProjectId == _projectId);

                    if (milestone != null)
                    {
                        var steps = context.Step.Where(x => x.MilestoneId == milestone.Id).ToList();

                        foreach (var item in steps)
                        {
                            var assignedUser = context.Person.FirstOrDefault(x => x.Id == item.AssigneeUser);
                            var sprint = context.Sprint.FirstOrDefault(x => x.Id == item.SprintId);



                            returnList.Add(new ChartDetailViewModel()
                            {
                                AssigneeUserName = $"{assignedUser.Name} {assignedUser.Surname}",
                                SprintId = item.SprintId,
                                SprintName = sprint.Name,
                                TaskId = item.Id,
                                TaskName = item.Name,
                                TaskStatus = item.Status,
                                Workers = GetWorkersForChartDetail(item.Id)
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

        public List<DoingWhatViewModel> GetAllDoingWhat(int _projectId)
        {
            try
            {
                var returnList = new List<DoingWhatViewModel>();

                using (var context = new ProjectManagementEntities())
                {
                    var stepPersonDetails = context.StepPersonDetail.Where(x => x.EndDate == null).ToList();

                    foreach (var item in stepPersonDetails)
                    {
                        var stepPerson = context.StepPerson.FirstOrDefault(x => x.Id == item.StepPersonId);
                        var step = context.Step.FirstOrDefault(x => x.Id == stepPerson.StepId);
                        var person = context.Person.FirstOrDefault(x => x.Id == stepPerson.PersonId);
                        var sprint = context.Sprint.FirstOrDefault(x => x.Id == step.SprintId);
                        var milestone = context.Milestone.FirstOrDefault(x => x.Id == step.MilestoneId);
                        var project = context.Project.FirstOrDefault(x => x.Id == sprint.ProjectId);

                        if (project.Id != _projectId)
                            continue;

                        var team = context.Team.FirstOrDefault(x => x.Id == person.TeamId);

                        returnList.Add(new DoingWhatViewModel()
                        {
                            NameSurname = $"{person.Name} {person.Surname}",
                            StepName = step.Name,
                            SprintName = sprint.Name,
                            MileStoneName = milestone.Name,
                            ProjectName = project.Name,
                            StartDate = item.StartDate,
                            TeamName = team.Name,
                            KacSaattirCalisiyor = MilliSecondsToHumanReadableDateTime(GetMillisecondsBetweenTwoDate(item.StartDate, DateTime.Now))
                        });
                    }
                }

                return returnList;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}

public class GetPercentage
{
    public string Butce { get; set; }
    public string Ilerleme { get; set; }
    public string Gerceklesen { get; set; }
    public string Kazanilan { get; set; }
}
public class Notification : Step
{
    public int Kalan { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
}