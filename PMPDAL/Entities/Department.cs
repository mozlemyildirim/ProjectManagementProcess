using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int Status { get; set; }
        public int HeadOfDepartmentId { get; set; }
    }
}
