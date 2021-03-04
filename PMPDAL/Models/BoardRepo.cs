using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class BoardRepo
    {
        public int SprintId { get; set; }
        public string DetailName { get; set; }
        public string MilestoneName { get; set; }
        public string AssigneeUserName { get; set; }
        public string Descripton { get; set; }
        public List<string> AssignedPersonNames { get; set; }
        public int StepId { get; set; }
        public int BoardId { get; set; }
    }
}
