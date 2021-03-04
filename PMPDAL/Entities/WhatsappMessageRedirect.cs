using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class WhatsappMessageRedirect
    {
        [Key]
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public int RedirectedUserId { get; set; }
    }
}
