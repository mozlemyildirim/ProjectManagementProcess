using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class ChartDetailViewModel
    {
        public int SprintId { get; set; }
        public string SprintName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string AssigneeUserName { get; set; }
        public string Workers { get; set; }
        public int TaskStatus { get; set; }
    }
}
