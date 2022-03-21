using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class MyAppointmentModel
    {
        [Key]
        public long MyAppointmentID { get; set; }
        /*FOREIGN KEYS*/
        public long ScheduleID { get; set; }

        public long MedicalHistoryID { get; set; }

        public long UserID { get; set; }

        public string SpecificProblem { get; set; }

    }
}