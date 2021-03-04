using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class BacklogViewModel
    {
        public List<SprintRepo> SprintList { get; set; }
        public List<BacklogDetail> Details { get; set; }
    }

    public class BacklogDetail
    {
        public int SprintId { get; set; }
        public string DetailName { get; set; }
        public string MilestoneName { get; set; }
        public string AssigneeUserName { get; set; }
        public string Descripton { get; set; }
        public List<string> AssignedPersonNames { get; set; }
        public int StepId { get; set; }
        public int MilestoneId { get; set; }
        public DateTime StepStartDate { get; set; }
        public DateTime StepEndDate { get; set; }
        public int StepStatus { get; set; }
    }
}
