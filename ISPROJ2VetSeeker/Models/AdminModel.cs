using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class AdminModel
    {
        public long AuditLogID { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        public DateTime DateOfLogin { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}")]
        public DateTime DateOfLogout { get; set; }

    }
}