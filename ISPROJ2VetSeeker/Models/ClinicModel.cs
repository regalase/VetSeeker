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
        [Display(Name = "Clinic Email")]
        [Required]
        public string ClinicEmail { get; set; }

        [RegularExpression(@"^09(73|74|05|06|15|16|17|26|27|35|36|37|79|38|07|08|09|10|12|18|19|20|21|28|29|30|38|39|89|99|22|23|32|33)\d{3}\s?\d{4}", ErrorMessage = "Invalid Format")]
        [MaxLength(11)]
        [Required]
        public string ClinicContactNo { get; set; }

        [Required]
        public string Longitude { get; set; }

        [Required]
        public string Latitude { get; set; }

        //public bool Status { get; set; }

    }
}