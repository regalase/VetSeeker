using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GMap;

namespace ISPROJ2VetSeeker.Models
{
    public class ClinicModel
    {
        [Key]
        public long ClinicID { get; set; }

        public long UserID { get; set; }

        [Display(Name = "Clinic Name")]
        [Required]
        public string ClinicName { get; set; }

        [Display(Name = "Unit House No.")]
        [Required]
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

        //public bool Status { get; set; }

    }
}