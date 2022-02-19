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

        /*DETAILS*/
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string Status { get; set; }

    }
}