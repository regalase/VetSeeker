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
        //FOREIGN KEYS//
        public long UserID { get; set; }

        public int PetID { get; set; }

        public long ClinicID { get; set; }

        //Only able to create schedules within the next year
        [DataType(DataType.Date)]
        [CustomSchedDate(ErrorMessage = "Schedules must be scheduled within the next year")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Status { get; set; }

    }
    public class CustomSchedDateAttribute : RangeAttribute
    {
        public CustomSchedDateAttribute()
          : base(typeof(DateTime),
                  DateTime.Now.AddDays(1).ToShortDateString(),
                  DateTime.Now.AddYears(1).ToShortDateString())
        { }
    }
}