using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Detail { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
    }
}
