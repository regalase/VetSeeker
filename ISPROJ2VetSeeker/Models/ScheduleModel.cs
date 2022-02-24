using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ScheduleModel
    {

        public ScheduleModel()
        {
            Date = DateTime.Now;
        }

        [Key]
        public long ScheduleID { get; set; }
        /*FOREIGN KEYS*/
        public long UserID { get; set; }

        public long ClinicID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Status { get; set; }

    }
}