using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Professional Fee")]
        [Required]
        public decimal ProfessionalFee { get; set; }

        [Display(Name = "Service Fee")]
        public decimal ServiceFee { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public PetModel PetModel { get; set; }
        public List<ServiceTypeModel> ServiceTypeModels { get; set; }

        [Display(Name = "Diagnosis")]
        [Required]
        public string Diagnosis { get; set; }
    }
}