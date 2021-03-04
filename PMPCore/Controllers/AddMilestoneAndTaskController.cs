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
    public class AddMilestoneAndTaskController : Controller
    {
        public IActionResult Index()
        {
            var milestoneDetailList = new MilestoneViewModel();
            milestoneDetailList.MilestoneList = MilestoneDB.GetInstance().GetAllMilestone(JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id);
            milestoneDetailList.PersonList = PersonDB.GetInstance().GetAllPerson();
            //milestoneDetailList.SprintList = SprintDB.GetInstance().GetAllSprint();
            var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
            milestoneDetailList.ProjectList = ProjectDB.GetInstance().GetAllProject(personId);



            return View(milestoneDetailList);
        }



        public JsonResult SaveMilestone(string _titleMilestone, string _endDate, string _startDate, string _description, int _projectID)

        {
            try
            {
                var milestone = new Milestone()
                {
                    Name = _titleMilestone,
                    Description = _description,
                    StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ProjectId = _projectID
                };

                var result = MilestoneDB.GetInstance().SaveMilestone(milestone);
                return Json(result != null);

            }
            catch (Exception exc)
            {

                throw exc;
            }
        }





        public JsonResult SaveProject(string _titleProject, string _code, string _shortname, string _description, int _personID)

        {
            try
            {
                var project = new Project()
                {
                    Name = _titleProject,
                    Code = _code,
                    ShortName = _shortname,
                    Description = _description,
                    ProjectLeader = _personID


                };

                var result = ProjectDB.GetInstance().SaveProject(project);
                return Json(result != null);

            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
    }
}