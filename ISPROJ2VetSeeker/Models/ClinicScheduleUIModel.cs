using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ClinicScheduleUIModel
    {

        public string ClinicName { get; set; }

        public List<ScheduleModel> ScheduleModels { get; set; }

    }
}