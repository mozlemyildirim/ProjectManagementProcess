using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public int ProjectLeader { get; set; }
        public int Status { get; set; }

    }
}
