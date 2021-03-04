using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class TaskUser
    {
        [Key]
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int PersonId { get; set; }
    }
}
