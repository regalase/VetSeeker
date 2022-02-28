using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ClientViewScheduleModel
    {

        public ClientViewScheduleModel()
        {
            ClinicScheduleUIModels = new List<ClinicScheduleUIModel>();
        }

        public List<ClinicScheduleUIModel> ClinicScheduleUIModels { get; set;} 

        public int SelectedPetId { get; set; }
        public int SelectedScheduleID { get; set; }
    }
}