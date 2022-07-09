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

        //Foreign key//
        public int UserID { get; set; }
        public List<UserModel> Users { get; set; }

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ])+", ErrorMessage = "Invalid Pet Name")]
        [MinLength(2, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too short")]
        [Display(Name = "Pet Name")]
        [Required]
        public String PetName { get; set; }

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ])+", ErrorMessage = "Invalid Breed Name")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too short")]
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
        public DateTime Birthday { get; set; }

        //accepts only numbers and exactly 15 of them
        [RegularExpression(@"^[0-9]{15}$", ErrorMessage = "Invalid Pet Chip No")]
        [Display(Name = "Pet Chip Number")]
        public string PetChipNo { get; set; }

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ])+", ErrorMessage = "Invalid Guardian Name")]
        [MinLength(2, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too long")]
        [Display(Name = "Guardian")]
        [Required]
        public String Guardian { get; set; }

        public string PetProfilePic { get; set; }

        [MaxLength(100)]
        public string Remarks { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}