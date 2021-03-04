using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PMPDAL.Entities
{
    public class WhatsappComminucation
    {
        [Key]
        public int Id { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Date { get; set; }
    }
}
