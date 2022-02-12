﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ClinicModel
    {
        [Key]
        public long ClinicID { get; set; }

        public long UserID { get; set; }

        [Display(Name = "Unit House No.")]
        [Required]
        public int UnitHouseNo { get; set; }

        [Display(Name = "Street")]
        [Required]
        public string Street { get; set; }

        [Display(Name = "Barangay")]
        [Required]
        public string Barangay { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

    }
}