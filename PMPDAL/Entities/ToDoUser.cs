using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class ToDoUser
    {
        [Key]
        public int Id { get; set; }
        public int ToDoId { get; set; }
        public int PersonId { get; set; }
    }
}
