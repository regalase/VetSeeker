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
            PetModels = new List<PetModel>();
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

        public List<PetModel> PetModels { get; set; }

        public int selectedPetID { get; set; }


    }
}