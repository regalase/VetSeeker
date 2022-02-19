using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class CreateScheduleUIModel
    {

        public CreateScheduleUIModel()
        {
            ClinicModels = new List<ClinicModel>();
            ScheduleModel = new ScheduleModel();
        }

        public List<ClinicModel> ClinicModels { get; set; }
        public ScheduleModel ScheduleModel { get; set; }


    }
}