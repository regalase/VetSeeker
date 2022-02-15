using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class UserTypeModel
    {
        [Key]
        public int TypeID { get; set; }

        public string Type { get; set; }

    }
}