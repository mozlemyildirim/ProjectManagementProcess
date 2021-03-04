using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class Step
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MilestoneId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int AssigneeUser { get; set; }
        public int SprintId { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
    }
}
