using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ClinicScheduleUIModel
    {
        public ClinicScheduleUIModel()
        {
            ScheduleModels = new List<ScheduleModel>();
        }

        public long ClinicID { get; set; }
        public long UserID { get; set; }
        public string ClinicName { get; set; }

        public string UnitHouseNo { get; set; }

        public string Street { get; set; }

        public string Baranggay { get; set; }

        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public List<ScheduleModel> ScheduleModels { get; set; }

    }
}