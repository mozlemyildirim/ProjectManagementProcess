using PMPDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMPDAL.Models
{
    public class StartStopViewModel : StepPerson
    {
        public string StepName { get; set; }
        public string MilestoneName { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }
    }
}
