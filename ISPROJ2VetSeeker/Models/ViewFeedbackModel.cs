using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class ViewFeedbackModel
    {
        public int MyAppointmentID { get; set; }
        public string Username { get; set; }
        public string Feedback { get; set; }
        public decimal Rating { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
    }
}