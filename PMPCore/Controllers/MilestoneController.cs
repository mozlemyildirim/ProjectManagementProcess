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

namespace PMPCore.Controllers
{
    public class MilestoneController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllMilestoneSelect()
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var milestone = MilestoneDB.GetInstance().GetAllMilestoneSelect(_projectId);



                return Json(milestone);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetKullaniciById(int _kullaniciId)
        {
            try
            {
                var kullanici = PersonDB.GetInstance().GetPersonById(_kullaniciId);
                var returnObj = kullanici.Name + " " + kullanici.Surname;

                return Json(returnObj);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetPersonById(int _personId)
        {
            try
            {
                var result = PersonDB.GetInstance().GetPersonById(_personId);
                return Json(result);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult GetAllProjeler()
        {
            try
            {
                var personId = JsonConvert.DeserializeObject<Person>(HttpContext.Session.GetString("ActivePerson")).Id;
                var projeler = ProjectDB.GetInstance().GetAllProject(personId);

                return Json(projeler);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }



        public JsonResult SaveMilestone(string _titleMilestone, string _endDate, string _startDate, string _description, int _milestoneId)
        {
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;

            try
            {
                if (_milestoneId == 0)
                {
                    var milestone = new Milestone()
                    {
                        Name = _titleMilestone,
                        Description = _description,
                        StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ProjectId = _projectId,
                        Status = 1


                    };

                    var result = MilestoneDB.GetInstance().SaveMilestone(milestone);
                    return Json(result != null);
                }
                else
                {
                    var milestone = new Milestone()
                    {
                        Name = _titleMilestone,
                        Description = _description,
                        StartDate = DateTime.ParseExact(_startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(_endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ProjectId = _projectId,
                        Id = _milestoneId,
                        Status = 1
                    };

                    var result = MilestoneDB.GetInstance().UpdateMilestone(milestone);
                    return Json(result != null);
                }

            }
            catch (Exception exc)
            {

                throw exc;
            }
        }


        public JsonResult GetMilestoneById(int _milestoneId)
        {
            try
            {
                var milestone = MilestoneDB.GetInstance().GetMilestoneById(_milestoneId);

                return Json(milestone);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }

        public JsonResult DeleteMilestone(int _projeId)
        {
            try
            {
                var result = MilestoneDB.GetInstance().DeleteMilestone(_projeId);
                return Json(result);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
    }
}