using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ViewAppointmentsModel
    {
        public int PetID { get; set; }

        public int MyAppointmentID { get; set; }

        public string PetName { get; set; }

        public string ClinicName { get; set; }

        public int ClinicID { get; set; }

        public int ScheduleID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int VetID { get; set; }

        public int UserID { get; set; }

        public string Status { get; set; }


    }
}