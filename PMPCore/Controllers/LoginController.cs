using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PMPDAL;
using Microsoft.AspNetCore.Http;


namespace PMPCore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult ControlLogin(string _email, string _password)
        {
            try
            {
                var person = PersonDB.GetInstance().ControlPerson(_email, _password);

                if (person != null)
                {
                    HttpContext.Session.SetString("ActivePerson", JsonConvert.SerializeObject(person));

                    // var projects = ProjectDB.GetInstance().GetAllProject();

                    // if (projects.Count > 0)
                    // {
                    //        HttpContext.Session.SetString("SelectedProject", JsonConvert.SerializeObject(projects.FirstOrDefault()));
                    // }

                    return Json(true);
                }

                return Json(false);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("ActivePerson");
            HttpContext.Session.Remove("SelectedProject");
            return RedirectToAction("Index");
        }
    }
}