using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
    }
}
