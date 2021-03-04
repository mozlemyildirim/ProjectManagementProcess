using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;
using PMPDAL.Models;

namespace PMPCore.Controllers
{
    public class BacklogController : BaseController
    {
        public IActionResult Index()
        {
            var backLogDetailList = new List<BacklogDetail>();
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
            var stepList = StepDB.GetInstance().GetAllStep(_projectId);

            foreach (var item in stepList)
            {
                var assigneeUser = PersonDB.GetInstance().GetPersonById(item.AssigneeUser);
                var milestone = MilestoneDB.GetInstance().GetMilestoneById(item.MilestoneId);

                if (milestone == null)
                    continue;

                backLogDetailList.Add(new BacklogDetail()
                {
                    SprintId = item.SprintId,
                    AssigneeUserName = $"{assigneeUser.Name} {assigneeUser.Surname}",
                    Descripton = item.Description,
                    DetailName = item.Name,
                    MilestoneName = milestone.Name,
                    AssignedPersonNames = StepDB.GetInstance().GetStepPersonsByStepId(item.Id),
                    StepId = item.Id,
                    StepStatus = item.Status
                });
            }

            var viewModel = new BacklogViewModel()
            {
                SprintList = SprintDB.GetInstance().GetAllSprintByProjectId(_projectId),
                Details = backLogDetailList
            };

            return View(viewModel);
        }


        public JsonResult GetSprintById(int _sprintId)
        {
            try
            {
                var result = SprintDB.GetInstance().GetSprintById(_sprintId);
                return Json(result);
            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }
        public JsonResult SaveSprint(string _titleSprint, string _endDate, string _startDate, string _description, int _sprintId)
        {
            try
            {
                if (Convert.ToInt32(_sprintId) == 0)
                {
                    var sprint = new Sprint()
                    {
                        Name = _titleSprint,
                        StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Description = _description,
                        Status = 1,
                        ProjectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id

                    };

                    var result = SprintDB.GetInstance().SaveSprint(sprint);
                    return Json(result != null);
                }
                else
                {
                    var sprint = new Sprint()
                    {
                        Name = _titleSprint,
                        StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Description = _description,
                        Status = 1,
                        Id = Convert.ToInt32(_sprintId)

                    };

                    var result = SprintDB.GetInstance().UpdateSprint(sprint);
                    return Json(result != null);
                }

            }
            catch (Exception exc)
            {

                throw exc;
            }
        }

        public JsonResult DeleteSprint(int _sprintId)
        {
            try
            {
                var result = SprintDB.GetInstance().DeleteSprint(_sprintId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult SaveBoardStepBySprintId(int _sprintId)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var result = BoardDB.GetInstance().SaveBoardStepBySprintId(_sprintId, JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id, userId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult ControlIfExistInPano(int _sprintId)
        {
            try
            {
                var result = BoardDB.GetInstance().ControlIfExistInPano(_sprintId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult StartSprint(int _sprintId)
        {
            try
            {
                var userId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var result = SprintDB.GetInstance().StartSprint(_sprintId, userId);
                return Json(result != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult StopSprint(int _sprintId)
        {
            try
            {
                var result = SprintDB.GetInstance().StopSprint(_sprintId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}