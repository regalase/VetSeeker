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
            BookedScheduleModel = new ScheduleModel();
            PetsForBooking = new List<PetModel>();
        }
        public List<ClinicScheduleUIModel> ClinicScheduleUIModels { get; set;} 
        public int SelectedPetId { get; set; }
        public int SelectedScheduleID { get; set; }
        public String SpecificProblem { get; set; }
        public ScheduleModel BookedScheduleModel { get; set; }

        public List<PetModel> PetsForBooking { get; set; }
        
    }
}