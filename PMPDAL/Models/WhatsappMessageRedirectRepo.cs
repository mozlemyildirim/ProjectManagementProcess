using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class WhatsappMessageRedirectRepo
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int DepartmentId { get; set; }
        public string RedirectDate { get; set; }
        public int Status { get; set; }
        public int RedirectedUserId { get; set; }
        public string Message { get; set; }
        public string DepartmentName { get; set; }
        public string RedirectedUserName { get; set; }
        public string SenderNumber { get; set; }
        public string MessageDate { get; set; }
    }
}
