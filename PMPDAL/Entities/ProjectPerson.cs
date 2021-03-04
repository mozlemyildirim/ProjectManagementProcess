using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class ProjectPerson
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PersonId { get; set; }
    }
}
