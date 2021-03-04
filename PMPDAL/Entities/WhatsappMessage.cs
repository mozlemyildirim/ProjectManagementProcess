using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class WhatsappMessage
    {
        [Key]
        public int Id { get; set; }
        public string MessageFrom { get; set; }
        public string MessageContent { get; set; }
        public DateTime Date { get; set; }
        public bool IsSeen { get; set; }
    }
}
