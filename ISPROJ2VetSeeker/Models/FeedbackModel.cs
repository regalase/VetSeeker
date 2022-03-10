using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class FeedbackModel
    {
        [Key]
        public long FeedbackID { get; set; }
        /*FOREIGN KEYS*/
        public long MyAppointmentID { get; set; }
        /*DETAILS*/

        [Display(Name = "Feedback")]
        [Required]
        public string Feedback { get; set; }

        [Display(Name = "Rating")]
        [Required]
        public decimal Rating { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

    }
}