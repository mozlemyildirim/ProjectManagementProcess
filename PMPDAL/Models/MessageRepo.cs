using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class MessageRepo
    {
        public int MessageId { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool IsFromMe { get; set; }
    }
}
