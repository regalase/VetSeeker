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
        public decimal ProfessionalFee { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}