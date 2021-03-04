using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;
using PMPDAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace PMPCore.Controllers
{
    public class GorevController : BaseController
    {
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult GetAllStep()
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var stepList = StepDB.GetInstance().GetAllStep(_projectId);
                var backLogDetailList = new List<BacklogDetail>();

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
                        MilestoneId = item.MilestoneId,
                        StepEndDate = Convert.ToDateTime(item.EndDate),
                        StepStartDate = Convert.ToDateTime(item.StartDate),
                        StepStatus = item.Status
                    });
                }

                var projectMilestones = MilestoneDB.GetInstance().GetAllMilestoneByProjectId(_projectId);
                var projectMilestoneIds = projectMilestones.Select(x => x.Id).ToList();
                backLogDetailList = backLogDetailList.Where(x => projectMilestoneIds.Contains(x.MilestoneId)).ToList();




                return Json(backLogDetailList);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllMilestone()
        {
            try
            {
                var milestoneList = StepDB.GetInstance().GetAllMilestone(JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject").ToString()).Id);

                return Json(milestoneList);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetAllNotification()
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var milestoneList = StepDB.GetInstance().GetAllNotification(personId);

                return Json(milestoneList);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllKullanici()
        {
            try
            {
                var kullanici = StepDB.GetInstance().GetAllKullanici();

                return Json(kullanici);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllSprint()
        {
            try
            {
                var sprint = StepDB.GetInstance().GetAllSprint(JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id);

                return Json(sprint);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetGorevById(int _gorevId)
        {
            try
            {
                var gorev = StepDB.GetInstance().GetGorevById(_gorevId);

                return Json(gorev);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult SaveStep(string _startDate, string _endDate, int _milestoneId, string _title, string _desc, int _assignedUserId, List<int> _workerIds, int _sprintID, int _stepId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    if (_stepId == 0)
                    {
                        var step = new Step()
                        {
                            AssigneeUser = _assignedUserId,
                            Description = _desc,
                            EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MilestoneId = _milestoneId,
                            Name = _title,
                            SprintId = _sprintID,
                            StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Status = 1
                        };
                        var result = StepDB.GetInstance().SaveStep(step, _workerIds);
                        if (result != null)
                        {
                            var personName = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Name;
                            var personSurname = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Surname;
                            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                            var _projectName = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Name;
                            var personEmail = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Email;
                            var stepName = result.Name;
                            var stepPersons = context.StepPerson.Where(x => x.StepId == result.Id).Select(x => x.PersonId).ToList();
                            var projectPerson = context.ProjectPerson.Where(x => x.ProjectId == _projectId).Select(x => x.PersonId).ToList();
                            var projectPersonMail = context.Person.FirstOrDefault(x => projectPerson.Contains(x.Id)).Email;
                            var personsMails = context.Person.Where(x => stepPersons.Contains(x.Id)).Select(x => x.Email).ToList();
                            var message = _projectName + " adlı projenin " + stepName + " adlı görevi " + personName + " " + personSurname + " tarafından " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde eklendi";
                            personsMails.Add(personEmail);
                            personsMails.Add(projectPersonMail);
                            personsMails = personsMails.Distinct().ToList();
                            SendEmail(message, personsMails);
                            return Json(result);
                        }

                        return Json(result != null);
                    }
                    else
                    {
                        var step = new Step()
                        {
                            AssigneeUser = _assignedUserId,
                            Description = _desc,
                            EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MilestoneId = _milestoneId,
                            Name = _title,
                            SprintId = _sprintID,
                            StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Id = _stepId
                        };
                        var result = StepDB.GetInstance().UpdateStep(step, _workerIds);
                        if (result != null)
                        {
                            var personName = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Name;
                            var personSurname = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Surname;
                            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                            var _projectName = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Name;
                            var personEmail = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Email;
                            var stepName = result.Name;
                            var stepPersons = context.StepPerson.Where(x => x.StepId == result.Id).Select(x => x.PersonId).ToList();
                            var projectPerson = context.ProjectPerson.Where(x => x.ProjectId == _projectId).Select(x => x.PersonId).ToList();
                            var projectPersonMail = context.Person.FirstOrDefault(x => projectPerson.Contains(x.Id)).Email;
                            var personsMails = context.Person.Where(x => stepPersons.Contains(x.Id)).Select(x => x.Email).ToList();
                            var message = _projectName + " adlı projenin " + stepName + " adlı görevi " + personName + " " + personSurname + " tarafından " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde güncellendi";
                            personsMails.Add(personEmail);
                            personsMails.Add(projectPersonMail);
                            personsMails = personsMails.Distinct().ToList();
                            SendEmail(message, personsMails);
                            return Json(result);
                        }
                        return Json(result != null);
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteGorev(int _gorevId)
        {
            try
            {
                var result = StepDB.GetInstance().DeleteGorev(_gorevId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetStepPersonsByStepId(int _gorevId)
        {
            try
            {
                var result = StepDB.GetInstance().GetStepPersonListByStepId(_gorevId);

                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
    }
}