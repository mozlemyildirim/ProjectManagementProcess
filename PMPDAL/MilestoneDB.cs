using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMPDAL
{
    public class MilestoneDB
    {
        private static MilestoneDB instance = null;



        public static MilestoneDB GetInstance()
        {
            if (instance == null)
                instance = new MilestoneDB();
            return instance;
        }

        public Milestone GetMilestoneById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var milestone = context.Milestone.FirstOrDefault(x => x.Id == _id && x.Status > 0);
                    return milestone;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        public List<Milestone> GetAllMilestone(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.Milestone.Where(x => x.ProjectId == _projectId).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public Milestone SaveMilestone(Milestone _m)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    context.Milestone.Add(_m);
                    int numberOfInserted = context.SaveChanges();
                    return numberOfInserted > 0 ? _m : null;

                }



            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public Milestone UpdateMilestone(Milestone _m)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var milestone = context.Milestone.FirstOrDefault(x => x.Id == _m.Id);

                    if (milestone != null)
                    {
                        milestone.Name = _m.Name;
                        milestone.ProjectId = _m.ProjectId;
                        milestone.StartDate = _m.StartDate;
                        milestone.Description = _m.Description;
                        milestone.EndDate = _m.EndDate;
                        int numberOfUpdated = context.SaveChanges();

                        return numberOfUpdated > 0 ? milestone : null;
                    }
                    return null;

                }

            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public List<Milestone> GetAllMilestoneByProjectId(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var milestones = context.Milestone.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList();
                    return milestones;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public List<MilestoneDetail> GetMilestoneInfo(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var returnList = new List<MilestoneDetail>();
                    var milestones = context.Milestone.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList().OrderByDescending(x => x.Id).ToList();

                    foreach (var item in milestones)
                    {
                        var mileStoneStep = context.Step.Where(x => x.MilestoneId == item.Id && x.Status != 0).ToList();
                        var mileStoneFinished = context.Step.Where(x => x.MilestoneId == item.Id && x.Status == 3).ToList();
                        var percent = mileStoneFinished.Count() == 0 ? 0d : Math.Round(((double)mileStoneFinished.Count() * (double)100) / (double)mileStoneStep.Count(), 2);

                        var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                        var steps = context.Step.Where(x => x.StartDate != null && x.EndDate != null && x.Status != 0 && x.Status != 3 && x.EndDate < todayDate && x.MilestoneId == item.Id).ToList();

                        foreach (var step in steps)
                        {
                            step.StartDate = Convert.ToDateTime(step.EndDate).AddDays(1);
                            step.EndDate = DateTime.Now;
                        }

                        returnList.Add(new MilestoneDetail()
                        {
                            EndDate = item.EndDate,
                            StartDate = item.StartDate,
                            FinishPercent = percent,
                            MilestoneName = item.Name,
                            DelayedTaskList = steps
                        });
                    }

                    return returnList;
                }
            }
            catch (System.Exception exc)
            {

                throw exc;
            }
        }

        public class MilestoneDetail
        {
            public string MilestoneName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public double FinishPercent { get; set; }
            public List<Step> DelayedTaskList { get; set; }
        }


        public List<Milestone> GetAllMilestoneSelect(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.Milestone.Where(x => x.Status > 0 && x.ProjectId == _projectId).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public Person GetPersonById(int _id)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var person = context.Person.FirstOrDefault(x => x.Id == _id);
                    return person;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        public List<Project> GetAllProject()
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var list = context.Project.Where(x => x.Status == 1).ToList();
                    return list;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }



        public bool DeleteMilestone(int _projeId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var milestone = context.Milestone.FirstOrDefault(x => x.Id == _projeId);

                    if (milestone != null)
                    {
                        milestone.Status = 0;
                        int numberOfDeleted = context.SaveChanges();

                        return numberOfDeleted > 0;
                    }

                }
                return false;

            }
            catch (System.Exception exc)
            {

                throw exc;
            }



        }

        public List<DateTime> GetProjectStartAndEndDate(int _projectId)
        {
            try
            {
                using (var context = new ProjectManagementEntities())
                {
                    var mileStoneList = context.Milestone.Where(x => x.Status > 0 && x.ProjectId == _projectId).ToList();
                    var startDate = mileStoneList.Select(x => x.StartDate).ToList().OrderBy(x => x).ToList().FirstOrDefault();
                    var endDate = mileStoneList.Select(x => x.EndDate).ToList().OrderByDescending(x => x).ToList().FirstOrDefault();

                    return new List<DateTime>() { startDate, endDate };
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public string GetTotalProjectPercent(int _projectId)
        {
            // try
            // {
            //        using (var context = new ProjectManagementEntities())
            //        {
            //               var milestones = context.Milestone.Where(x => x.ProjectId == _projectId && x.Status > 0).ToList();
            //               var projeKacGun = (milestones.OrderByDescending(x => x.EndDate).FirstOrDefault().EndDate - milestones.OrderBy(x => x.StartDate).FirstOrDefault().StartDate).TotalDays;
            //               int bitenGunSayisi = 0;

            //               foreach (var item in milestones)
            //               {
            //                      var milestoneKacGun = (item.EndDate - item.StartDate).TotalDays;

            //                      var steps = context.Step.Where(x => x.MilestoneId == item.Id && x.Status > 0).ToList();
            //                      var bitenStepSayisi = steps.Where(x => x.Status == 3).ToList().Count;
            //                      var finisedPercent = (100 / steps.Count) * bitenStepSayisi;

            //                      var milestoneBitenGunSayisi = (100 / milestoneKacGun) * finisedPercent; //(milestoneKacGun * finisedPercent) / 100d;
            //               }
            //        }
            return "";
            // }
            // catch (Exception exc)
            // {
            //        throw exc;
            // }
        }
    }
}
