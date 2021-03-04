using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class DepartmentRepo
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int Status { get; set; }
        public int HeadOfDepartmentId { get; set; }
        public string HeadOfDepartmentName { get; set; }
    }
}
