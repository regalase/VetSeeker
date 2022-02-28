using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class CompleteScheduleModel
    {
        public CompleteScheduleModel()
        {
            ScheduleModel = new ScheduleModel();
            PetModel = new PetModel();
            ServiceTypeModels = new List<ServiceTypeModel>();
            SelectedService = new ServiceTypeModel();
        }

        public int SelectedScheduleID { get; set; }
        public int MyAppointmentID { get; set; }
        public int SelectedPetID { get; set; }
        public ServiceTypeModel SelectedService { get; set; }

        public ScheduleModel ScheduleModel { get; set; }

        public decimal ProfessionalFee { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalPrice { get; set; }

        public PetModel PetModel { get; set; }
        public List<ServiceTypeModel> ServiceTypeModels { get; set; }
        public string Diagnosis { get; set; }
    }
}