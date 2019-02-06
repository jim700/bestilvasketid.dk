using System;
using System.Collections.Generic;

namespace bestilvasketid.dk.Models
{
    public class ScheduleModel
    {
        public DateTime DateTime { get; set; }

        public int Status { get; set; }

        public string ShowUserID { get; set; }

        public int Machine { get; set; }

        readonly List<string> StatusList = new List<string> { "Free", "Taken", "OutOfOrder", "Technician" };
    }
}