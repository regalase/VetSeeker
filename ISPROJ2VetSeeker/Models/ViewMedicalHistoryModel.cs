using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ViewMedicalHistoryModel
    {
        public int MedicalHistoryID { get; set; }

        public int PetID { get; set; }

        public int ServiceTypeID { get; set; }

        public string Diagnosis { get; set; }

        public int UserID { get; set; }

        public string ServiceName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNo { get; set; }

        public string Email { get; set; }

        public string PetName { get; set; }

    }
}