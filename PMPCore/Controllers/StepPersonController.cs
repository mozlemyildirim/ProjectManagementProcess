using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using PMPDAL.Entities;

namespace PMPCore.Controllers
{
    public class StepPersonController : BaseController
    {
        public IActionResult Index()
        {
            var user = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson"));
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
            var returnList = StepDB.GetInstance().GetStepPersons(user.Id, _projectId);
            return View(returnList);
        }

        public JsonResult ChangeStatus(int _stepPersonId, string _status, string _desc)
        {
            try
            {
                var changed = StepDB.GetInstance().SaveStepPersonStatus(_stepPersonId, _status, _desc);
                return Json(changed != null);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}