using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class BoardStep
    {
        [Key]
        public int Id { get; set; }
        public int BoardId { get; set; }
        public int StepId { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
