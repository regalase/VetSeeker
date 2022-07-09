using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ServiceTypeModel
    {
        [Key]
        public long ServiceTypeID { get; set; }
        /*FOREIGN KEYS*/
        public long UserID { get; set; }
        /*DETAILS*/

        [Display(Name = "Service Name")]
        [Required]
        public string ServiceName { get; set; }

        [Display(Name = "Service Description")]
        [Required]
        public string ServiceDescription { get; set; }

        [Display(Name = "Price")]
        [Required]
        public decimal Price { get; set; }
        public string ClinicName { get; set; }

        public string DropdownDisplayText { get; set; }
        public string Status { get; set; }

    }
}