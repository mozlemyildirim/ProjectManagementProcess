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

namespace PMPCore.Controllers
{
    public class BoardController : BaseController
    {
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult GetGorevByBoardId(int _sprintId)
        {
            try
            {
                var stepList = StepDB.GetInstance().GetGorevBySprintId(_sprintId);
                var backLogDetailList = new List<BacklogDetail>();

                foreach (var item in stepList)
                {
                    var assigneeUser = PersonDB.GetInstance().GetPersonById(item.AssigneeUser);
                    var milestone = MilestoneDB.GetInstance().GetMilestoneById(item.MilestoneId);

                    backLogDetailList.Add(new BacklogDetail()
                    {
                        SprintId = item.SprintId,
                        AssigneeUserName = $"{assigneeUser.Name} {assigneeUser.Surname}",
                        Descripton = item.Description,
                        DetailName = item.Name,
                        MilestoneName = milestone.Name,
                        AssignedPersonNames = StepDB.GetInstance().GetStepPersonsByStepId(item.Id),
                        StepId = item.Id
                    });
                }
                return Json(backLogDetailList);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllStepForPano()
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var boardList = BoardDB.GetInstance().GetBoardByProjectId(_projectId);
                var boardDetailList = new List<BoardRepo>();
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;

                foreach (var item in boardList)
                {
                    var stepList = BoardDB.GetInstance().GetStepByBoardId(item.Id, personId);

                    foreach (var step in stepList)
                    {
                        var assigneeUser = PersonDB.GetInstance().GetPersonById(step.AssigneeUser);
                        var milestone = MilestoneDB.GetInstance().GetMilestoneById(step.MilestoneId);

                        if (milestone == null)
                            continue;

                        boardDetailList.Add(new BoardRepo()
                        {
                            SprintId = step.SprintId,
                            AssigneeUserName = $"{assigneeUser.Name} {assigneeUser.Surname}",
                            Descripton = step.Description,
                            DetailName = step.Name,
                            MilestoneName = milestone.Name,
                            AssignedPersonNames = StepDB.GetInstance().GetStepPersonsByStepId(step.Id),
                            StepId = step.Id,
                            BoardId = item.Id
                        });
                    }
                }
                return Json(boardDetailList);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult UpdateBoardStep(int _stepId, int _boardId, string _desc)
        {
            _desc = _desc == null ? "" : _desc;
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                    var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                    var _projectName = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Name;
                    var updatedBoard = BoardDB.GetInstance().UpdateBoardStep(_stepId, _boardId, _projectId, personId, _desc);
                    if (updatedBoard == true)
                    {
                        var personEmail = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Email;
                        var stepName = context.Step.FirstOrDefault(x => x.Id == _stepId).Name;
                        var stepPersons = context.StepPerson.Where(x => x.StepId == _stepId).Select(x => x.PersonId).ToList();
                        var projectPerson = context.ProjectPerson.Where(x => x.ProjectId == _projectId).Select(x => x.PersonId).ToList();
                        var projectPersonMail = context.Person.FirstOrDefault(x => projectPerson.Contains(x.Id)).Email;
                        var personsMails = context.Person.Where(x => stepPersons.Contains(x.Id)).Select(x => x.Email).ToList();
                        var personName = context.Person.FirstOrDefault(x => x.Id == personId).Name;
                        var personSurname = context.Person.FirstOrDefault(x => x.Id == personId).Surname;
                        var boardName = context.Board.FirstOrDefault(x => x.Id == _boardId).Name;
                        var message = "";
                        if (_desc != "")
                        {
                           message = _projectName+" adlı projenin "+stepName + " adlı görevi " + _desc + " açıklamasıyla " + boardName + " panosuna " + personName +" "+ personSurname + " tarafından " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde taşındı.";
                        }
                        else
                        {
                            message = _projectName + " adlı projenin " + stepName + " adlı görevi " + boardName + " panosuna " + personName + " " + personSurname + " tarafından " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde taşındı.";
                        }
                        personsMails.Add(personEmail);
                        personsMails.Add(projectPersonMail);
                        personsMails=personsMails.Distinct().ToList();
                        SendEmail(message, personsMails);
                        return Json(updatedBoard);
                    }
                    return Json(updatedBoard);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllBoardByProjectId()
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var board = BoardDB.GetInstance().GetAllBoardByProjectId(_projectId);
                board = board.OrderBy(x => x.Id).ToList();
                return Json(board);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SavePano(string _panoAdi, int _panoId, bool _isDescRequired)
        {
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
            try
            {
                if (_panoId == 0)
                {
                    var pano = new Board()
                    {
                        Name = _panoAdi,
                        ProjectId = _projectId,
                        Status = 1,
                        IsDescriptionRequired = _isDescRequired ? 1 : 0
                    };

                    var result = BoardDB.GetInstance().SavePano(pano);
                    return Json(result != null);
                }
                else
                {
                    var pano = new Board()
                    {
                        Name = _panoAdi,
                        Id = _panoId,
                        ProjectId = _projectId,
                        IsDescriptionRequired = _isDescRequired ? 1 : 0
                    };

                    var result = BoardDB.GetInstance().UpdatePano(pano);
                    return Json(result != null);
                }
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }


        public JsonResult GetPanoById(int _boardId)
        {
            try
            {
                var board = BoardDB.GetInstance().GetPanoById(_boardId);

                return Json(board);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult DeletePano(int _panoId)
        {
            try
            {
                var result = BoardDB.GetInstance().DeletePano(_panoId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult FinishSprint()
        {
            try
            {

                var result = BoardDB.GetInstance().FinishSprint(JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetStepHoverInfo(int _stepId, int _boardId)
        {
            try
            {
                var str = BoardDB.GetInstance().GetStepHoverInfo(_stepId, _boardId);
                return Json(str);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}