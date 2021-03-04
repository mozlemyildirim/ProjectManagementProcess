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
    public class DoingWhatController : BaseController
    {
        public IActionResult Index()
        {
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
            var list = StepDB.GetInstance().GetAllDoingWhat(_projectId);
            return View(list);
        }
    }
}