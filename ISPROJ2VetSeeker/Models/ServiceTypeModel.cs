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
        public string ServiceName { get; set; }

        public decimal Price { get; set; }

        public string DropdownDisplayText { get; set; }

    }
}