using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPCore.Models;
using PMPDAL;
using PMPDAL.Entities;

namespace PMPCore.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            var returnList = new List<HomeViewModel>();
            var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
            var list = ProjectDB.GetInstance().GetAllProject(personId);

            list.ForEach(item =>
            {
                var assignedPerson = PersonDB.GetInstance().GetPersonById(item.ProjectLeader);
                var step = StepDB.GetInstance().GetAllNotificationByProjectId(item.Id);
                returnList.Add(new HomeViewModel()
                {
                    AssigneeUserName = $"{assignedPerson.Name} {assignedPerson.Surname}",
                    Code = item.Code,
                    Description = item.Description,
                    Id = item.Id,
                    Name = item.Name,
                    ProjectLeader = item.ProjectLeader,
                    ShortName = item.ShortName,
                    Status = item.Status,
                    Kalan=step
                });
            });

            return View(returnList);
        }

        public class HomeViewModel : Project
        {
            public string AssigneeUserName { get; set; }
            public int Kalan { get; set; }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
