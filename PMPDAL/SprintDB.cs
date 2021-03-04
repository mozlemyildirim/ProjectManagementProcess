using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class SprintDB
    {
        private static SprintDB instance = null;

        public static SprintDB GetInstance()
        {
            if (instance == null)
                instance = new SprintDB();
            return instance;
        }

        public List<SprintRepo> GetAllSprintByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<SprintRepo>();
                    var sprints = context.Sprint.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList();

                    foreach (var item in sprints)
                    {
                        var isRunning = item.Status == 2;

                        var sprint = new SprintRepo()
                        {
                            Description = item.Description,
                            EndDate = item.EndDate,
                            Id = item.Id,
                            IsRunning = isRunning,
                            Name = item.Name,
                            ProjectId = item.ProjectId,
                            StartDate = item.StartDate,
                            Status = item.Status
                        };

                        returnList.Add(sprint);
                    }

                    return returnList;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public Sprint SaveSprint(Sprint _s)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Sprint.Add(_s);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _s : null;

                }

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public Sprint UpdateSprint(Sprint _s)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprint = context.Sprint.FirstOrDefault(x => x.Id == _s.Id);

                    if (sprint != null)
                    {
                        sprint.Description = _s.Description;
                        sprint.EndDate = _s.EndDate;
                        sprint.Name = _s.Name;
                        sprint.StartDate = _s.StartDate;

                        int numberOfUpdated = context.SaveChanges();

                        return numberOfUpdated > 0 ? sprint : null;
                    }

                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public Sprint GetSprintById(int _sprintId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprint = context.Sprint.FirstOrDefault(x => x.Id == _sprintId);

                    if (sprint != null)
                    {
                        var returnModel = new Sprint()
                        {
                            Description = sprint.Description == null ? "" : sprint.Description,
                            EndDate = sprint.EndDate,
                            Id = sprint.Id,
                            Name = sprint.Name == null ? "" : sprint.Name,
                            StartDate = sprint.StartDate
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

        public bool DeleteSprint(int _sprintId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprint = context.Sprint.FirstOrDefault(x => x.Id == _sprintId);

                    if (sprint != null)
                    {
                        sprint.Status = 0;
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

        public Sprint StartSprint(int _sprintId, int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprint = context.Sprint.FirstOrDefault(x => x.Id == _sprintId);
                    var tasks = context.Step.Where(x => x.SprintId == sprint.Id && x.Status > 0).ToList();
                    var board = context.Board.Where(x => x.ProjectId == sprint.ProjectId).ToList().OrderBy(x => x.Id).ToList().FirstOrDefault();

                    foreach (var item in tasks)
                    {
                        var taskWorkerIds = context.StepPerson.Where(x => x.StepId == item.Id).ToList().Select(x => x.PersonId).ToList();
                        taskWorkerIds.Add(_personId);
                        taskWorkerIds = taskWorkerIds.Distinct().ToList();

                        var boardstep = new BoardStep()
                        {
                            BoardId = board.Id,
                            Date = DateTime.Now,
                            Description = "",
                            PersonId = 0,
                            StepId = item.Id
                        };
                        context.BoardStep.Add(boardstep);
                        context.SaveChanges();

                        //foreach (var workerId in taskWorkerIds)
                        //{
                        //    var boardStep = new BoardStep()
                        //    {
                        //        BoardId = board.Id,
                        //        StepId = item.Id,
                        //        PersonId = workerId,
                        //        Description = "",
                        //        Date = DateTime.Now
                        //    };
                        //    context.BoardStep.Add(boardStep);
                        //    int numberOfInserted = context.SaveChanges();
                        //}
                    }

                    sprint.Status = 2;
                    context.SaveChanges();

                    return sprint;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public int StopSprint(int _sprintId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var sprint = context.Sprint.FirstOrDefault(x => x.Id == _sprintId);
                    var steps = context.Step.Where(x => x.SprintId == _sprintId && x.Status > 0).ToList();
                    var stepIds = steps.Select(x => x.Id).ToList();
                    var stepPersons = context.StepPerson.Where(x => stepIds.Contains(x.StepId)).ToList();
                    var stepPersonIds = stepPersons.Select(x => x.Id).ToList();
                    var stepPersonDetails = context.StepPersonDetail.Where(x => stepPersonIds.Contains(x.StepPersonId) && x.EndDate == null).ToList();

                    if (stepPersonDetails.Count > 0)
                        return -1;

                    foreach (var item in steps)
                    {
                        var boardStep = context.BoardStep.FirstOrDefault(x => x.StepId == item.Id);

                        if (boardStep != null)
                        {
                            context.BoardStep.Remove(boardStep);
                            context.SaveChanges();
                        }

                        item.Status = 3;
                        context.SaveChanges();
                    }

                    sprint.Status = 3;
                    context.SaveChanges();

                    return 1;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
    public class SprintRepo : Sprint
    {
        public bool IsRunning { get; set; }
    }
}