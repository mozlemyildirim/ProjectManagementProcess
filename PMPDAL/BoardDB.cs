using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class BoardDB
    {
        //deneme
        //deneme2
        private static BoardDB instance = null;

        public static BoardDB GetInstance()
        {
            if (instance == null)
                instance = new BoardDB();
            return instance;
        }

        public BoardStep SaveBoardStepBySprintId(int _sprintId, int _projectId, int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    int numberOfInserted = 0;
                    var boardStep = new BoardStep();
                    var gorev = context.Step.Where(x => x.SprintId == _sprintId && x.Status != 0).ToList();

                    foreach (var item in gorev)
                    {
                        item.Status = 2;

                        boardStep = new BoardStep()
                        {
                            StepId = item.Id,
                            BoardId = context.Board.FirstOrDefault(x => x.ProjectId == _projectId && x.Status == 1).Id,
                            PersonId = _personId,
                            Description = "",
                            Date = DateTime.Now
                        };

                        context.BoardStep.Add(boardStep);
                        numberOfInserted = context.SaveChanges();
                    }

                    return numberOfInserted > 0 ? boardStep : null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public List<Board> GetAllBoardByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boardList = context.Board.Where(x => x.ProjectId == _projectId && x.Status == 1).ToList();
                    return boardList;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public List<Step> GetStepByBoardId(int _boardId, int _personId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boardStepIds = context.BoardStep.Where(x => x.BoardId == _boardId).ToList().Select(x => x.StepId).ToList();
                    var stepList = context.Step.Where(x => boardStepIds.Contains(x.Id) && x.Status>0).ToList();

                    return stepList;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool ControlIfExistInPano(int _sprintId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    //using (var command = context.Database.Connection.CreateCommand())
                    //{
                    //    if (context.Database.Connection.State == ConnectionState.Closed)
                    //        context.Database.Connection.Open();

                    //    command.CommandText = $"SELECT COUNT(*) FROM \"BoardStep\" WHERE \"StepId\" IN ( SELECT \"Id\" FROM \"Step\" WHERE \"MilestoneId\" IN ( SELECT \"Id\" FROM \"Milestone\" WHERE \"ProjectId\" = ( SELECT DISTINCT \"Project\".\"Id\" FROM \"Step\" INNER JOIN \"Milestone\" ON \"Milestone\".\"Id\" = \"Step\".\"MilestoneId\" INNER JOIN \"Project\" ON \"Project\".\"Id\" = \"Milestone\".\"ProjectId\" WHERE \"Step\".\"SprintId\" = {_sprintId} ) ) )";
                    //    using (System.Data.Common.DbDataReader dr = command.ExecuteReader())
                    //    {
                    //        var tb = new DataTable();
                    //        tb.Load(dr);
                    //        var result = Convert.ToInt32(tb.Rows[0][0].ToString());
                    //        return result > 0;
                    //    }
                    //}

                    //var step = context.Step.Where(x => x.SprintId == _sprintId).ToList().Select(x => x.Id).ToList();
                    //var boardStep = context.BoardStep.Where(x => step.Contains(x.StepId)).ToList();

                    //return boardStep.Count > 0;
                    return false;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public List<Board> GetBoardByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boardList = context.Board.Where(x => x.ProjectId == _projectId).ToList();
                    return boardList;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public bool UpdateBoardStep(int _stepId, int _boardId, int _projectId, int _personId, string _desc)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boardStep = context.BoardStep.FirstOrDefault(x => x.StepId == _stepId);

                    if (boardStep != null)
                    {
                        boardStep.BoardId = _boardId;
                        boardStep.PersonId = _personId;
                        boardStep.Description = _desc;
                        boardStep.Date = DateTime.Now;

                        int numberOfUpdated = context.SaveChanges();

                        var allBoards = context.Board.Where(x => x.ProjectId == _projectId && x.Status == 1).ToList().OrderBy(x => x.Id).ToList();

                        if (_boardId == allBoards[2].Id)
                        {
                            var step = context.Step.FirstOrDefault(x => x.Id == _stepId);
                            step.Status = 3;
                            context.SaveChanges();
                        }
                        else if (_boardId == allBoards[1].Id)
                        {
                            var step = context.Step.FirstOrDefault(x => x.Id == _stepId);
                            step.Status = 2;
                            context.SaveChanges();
                        }
                        else
                        {
                            var step = context.Step.FirstOrDefault(x => x.Id == _stepId);
                            step.Status = 1;
                            context.SaveChanges();
                        }

                        var calisilanList = context.StepPerson.Where(x => x.StepId == _stepId).ToList();

                        if (calisilanList.Count > 0)
                        {
                            var calisanListId = calisilanList.Select(x => x.Id).ToList();
                            var calisanDetailList = context.StepPersonDetail.Where(x => calisanListId.Contains(x.StepPersonId) && x.EndDate == null).ToList();

                            foreach (var item in calisanDetailList)
                            {
                                item.EndDate = DateTime.Now;
                                item.Description = "Adım otomatik olarak bitirildi.";
                                context.SaveChanges();
                            }
                        }

                        return numberOfUpdated > 0;
                    }
                    return false;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public Board SavePano(Board _b)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Board.Add(_b);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _b : null;
                }

            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Board UpdatePano(Board _b)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var pano = context.Board.FirstOrDefault(x => x.Id == _b.Id);

                    if (pano != null)
                    {
                        pano.ProjectId = _b.ProjectId;
                        pano.Name = _b.Name;
                        pano.IsDescriptionRequired = _b.IsDescriptionRequired;

                        int numberOfUpdated = context.SaveChanges();

                        return numberOfUpdated > 0 ? pano : null;
                    }
                    return null;
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public Board GetPanoById(int _panoId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var board = context.Board.FirstOrDefault(x => x.Id == _panoId);

                    if (board != null)
                    {
                        var returnModel = new Board()
                        {
                            Name = board.Name,
                            ProjectId = board.ProjectId,
                            IsDescriptionRequired = board.IsDescriptionRequired
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

        public bool DeletePano(int _panoId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var board = context.Board.FirstOrDefault(x => x.Id == _panoId);

                    if (board != null)
                    {
                        board.Status = 0;
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

        public int FinishSprint(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boards = context.Board.Where(x => x.ProjectId == _projectId & x.Status == 1).ToList();
                    var boardIds = boards.Select(x => x.Id).ToList();
                    var boardSteps = context.BoardStep.Where(x => boardIds.Contains(x.BoardId)).ToList();
                    var boardStepIds = boardSteps.Select(x => x.StepId).ToList();
                    var steps = context.Step.Where(x => boardStepIds.Contains(x.Id)).ToList();
                    var stepIds = steps.Select(x => x.Id).ToList();
                    var stepPerson = context.StepPerson.Where(x => stepIds.Contains(x.StepId)).ToList();
                    var stepPersonIds = stepPerson.Select(x => x.Id).ToList();
                    var control = context.StepPersonDetail.FirstOrDefault(x => stepPersonIds.Contains(x.StepPersonId) && x.EndDate == null);

                    if (control != null)
                        return -1;

                    foreach (var item in steps)
                    {
                        item.Status = 3;
                        context.SaveChanges();
                    }

                    context.BoardStep.RemoveRange(boardSteps);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public string GetStepHoverInfo(int _stepId, int _boardId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var boardStepList = context.BoardStep.Where(x => x.StepId == _stepId && x.BoardId == _boardId).ToList().OrderByDescending(x => x.Id).ToList();

                    if (boardStepList.Count > 0)
                    {
                        var boardStep = boardStepList.FirstOrDefault();
                        var person = context.Person.FirstOrDefault(x => x.Id == boardStep.PersonId);
                        string returnStr = $"Bu adım {person.Name} {person.Surname} tarafından {boardStep.Date.ToString("dd/MM/yyyy HH:mm")} tarihinde bu panoya taşınmıştır.";

                        if (boardStep.Description != null && !String.IsNullOrEmpty(boardStep.Description))
                        {
                            returnStr += "<br>Açıklama: " + boardStep.Description;
                        }

                        return returnStr.Trim();
                    }

                    return "";
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

    }
}
