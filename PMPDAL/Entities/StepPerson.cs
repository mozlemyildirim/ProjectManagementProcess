using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class StepPerson
    {
        [Key]
        public int Id { get; set; }
        public int StepId { get; set; }
        public int PersonId { get; set; }
        public string Description { get; set; }
    }
}
