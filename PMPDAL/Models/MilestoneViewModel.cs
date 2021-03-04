using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class MilestoneViewModel
    {
        public List<Milestone> MilestoneList { get; set; }
        public List<PersonViewModel> PersonList { get; set; }
        public List<Sprint> SprintList { get; set; }
        public List<Project> ProjectList { get; set; }
    }
}
