using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class PetModel
    {
        public long PetID { get; set; }

        /*Foreign key*/
        public int UserID { get; set; }
        public List<UserModel> Users { get; set; }

        [Display(Name = "Pet Name")]
        [Required]
        public String PetName { get; set; }

        [Display(Name = "Breed")]
        [Required]
        public String Breed { get; set; }

        [Display(Name = "Sex")]
        [Required]
        public String Sex { get; set; }

        [Display(Name = "Color")]
        [Required]
        public String Color { get; set; }

        [Display(Name = "Birthday")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Pet Chip Number")]
        public string PetChipNo { get; set; }

        [Display(Name = "Guardian")]
        [Required]
        public String Guardian { get; set; }

        public string PetProfilePic { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}