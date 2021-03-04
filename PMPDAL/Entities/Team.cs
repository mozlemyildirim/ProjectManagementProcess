using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int Status { get; set; }

    }
}
