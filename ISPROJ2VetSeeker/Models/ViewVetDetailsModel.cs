using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ViewVetDetailsModel
    {
        public ViewVetDetailsModel()
        {
            ServiceTypeModels = new List<ServiceTypeModel>();
            ClinicModel = new ClinicModel();
            UserModel = new UserModel();
        }

        public List<ServiceTypeModel> ServiceTypeModels { get; set; }

        public ClinicModel ClinicModel { get; set; }

        public UserModel UserModel { get; set; }


    }
}