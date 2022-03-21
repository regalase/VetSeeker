using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class InvoiceModel
    {
        [Key]
        public long InvoiceID { get; set; }
        /*FOREIGN KEYS*/
        public long MyAppointmentID { get; set; }

        public long MedicalHistoryID { get; set; }
        /*DETAILS*/

        [Display(Name = "Professional Fee")]
        [Required]
        public decimal ProfessionalFee { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}