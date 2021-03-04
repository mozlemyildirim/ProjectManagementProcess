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
    public class RoadmapController : BaseController
    {
        public IActionResult Index()
        {
            var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
            var dateList = MilestoneDB.GetInstance().GetProjectStartAndEndDate(_projectId);
            var dddd = MilestoneDB.GetInstance().GetTotalProjectPercent(_projectId);
            return View(dateList);
        }

        public JsonResult SaveMilestone(string _titleMilestone, string _endDate, string _startDate, string _description, int _projectID, int _milestoneId)

        {
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
                        ProjectId = _projectID
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
                        ProjectId = _projectID,
                        Id = _milestoneId
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

        public JsonResult GetMilestoneInfo()
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var list = MilestoneDB.GetInstance().GetMilestoneInfo(_projectId);
                return Json(list);
            }
            catch (System.Exception exc)
            {
                throw exc;
            }
        }
        public JsonResult GetProjectPercentage()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                    var pct = StepDB.GetInstance().GetProjectPercentage(_projectId);

                    pct.Gerceklesen = pct.Gerceklesen == null || pct.Gerceklesen.Trim() == "" ? "0" : pct.Gerceklesen;

                    var yuzde = Math.Round(Convert.ToDouble(pct.Gerceklesen.Replace(".", ",").Trim(), CultureInfo.GetCultureInfo("tr-TR")) * 100, 2);

                    return Json(yuzde);
                }

            }
            catch (System.Exception exc)
            {
                throw exc;
            }
            /*
                SET @pct = 0; 
                SET @boardId = 0;
                SET @allItems = 0;
                SET @completedItems = 0;

                SET @boardId = (SELECT Id FROM Board AS B 
                WHERE ProjectId = 1
                LIMIT 2,1);

                SET @allItems = (SELECT COUNT(1) FROM Step AS S INNER JOIN Milestone AS MS ON MS.Id = S.MilestoneId WHERE MS.ProjectId = 1);
                SET @completedItems = (SELECT COUNT(1) FROM Step AS S INNER JOIN Milestone AS MS ON MS.Id = S.MilestoneId INNER JOIN BoardStep AS BS ON BS.StepId = S.Id  WHERE MS.ProjectId = 1 AND BS.BoardId = @boardId);
                SET @pct = (@completedItems * 100 / @allItems) / 100;
                SELECT @pct;
             */
        }

        public JsonResult GetChartDetail(string _milestoneName)
        {
            try
            {
                var _projectId = JsonConvert.DeserializeObject<Project>(HttpContext.Session.GetString("SelectedProject")).Id;
                var list = StepDB.GetInstance().GetChartDetail(_milestoneName, _projectId);
                return Json(list);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
