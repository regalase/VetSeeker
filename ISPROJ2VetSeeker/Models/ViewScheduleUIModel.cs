using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ViewScheduleUIModel
    {

        //scheduleId, userId, date, status, clinicId, clinicName
        public int ScheduleId { get; set; }

        public int UserID { get; set; }
        public DateTime Date { get; set; }

        public String Status { get; set; }

        public int ClinicId { get; set; }

        public String ClinicName { get; set; }
        public int MyAppoinmentId { get; set; }
        public String SpecificProblem { get; set; }

    }

}