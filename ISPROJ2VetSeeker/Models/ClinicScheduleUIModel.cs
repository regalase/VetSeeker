using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Clinic Name")]
        [Required]
        public string ClinicName { get; set; }

        [Display(Name = "Unit House Number")]
        public string UnitHouseNo { get; set; }

        [Display(Name = "Street")]
        [Required]
        public string Street { get; set; }

        [Display(Name = "Barangay")]
        [Required]
        public string Baranggay { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Required]
        public string Longitude { get; set; }

        [Required]
        public string Latitude { get; set; }

        public List<ScheduleModel> ScheduleModels { get; set; }

        public List<PetModel> PetModels { get; set; }

        public int selectedPetID { get; set; }

    }
}