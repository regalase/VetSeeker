using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class MedicalHistoryModel
    {
        [Key]
        public long MedicalHistoryID { get; set; }
        /*FOREIGN KEYS*/
        public long PetID { get; set; }

        public long ServiceTypeID { get; set; }
        /*DETAILS*/
        public string Diagnosis { get; set; }

    }
}