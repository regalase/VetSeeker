using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ScheduleModel
    {
        [Key]
        public long ScheduleID { get; set; }
        /*FOREIGN KEYS*/
        public long UserID { get; set; }
        /*DETAILS*/
        public DateTime Date { get; set; }

        public string Status { get; set; }

    }
}