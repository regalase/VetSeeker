using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISPROJ2VetSeeker.Models
{
    public class UserModel
    {
        [Key]
        public long UserID { get; set; }

        public long TypeID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [RegularExpression(@"^09(73|74|05|06|15|16|17|26|27|35|36|37|79|38|07|08|09|10|12|18|19|20|21|28|29|30|38|39|89|99|22|23|32|33)\d{3}\s?\d{4}", ErrorMessage = "Invalid Format")]
        [MaxLength(30)]
        public string MobileNo { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid format.")]
        public string Email { get; set; }

        [Display(Name = "UserName")]
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }

        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        [Display(Name = "Unit House No.")]
        [Required]
        public int UnitHouseNo { get; set; }

        [Display(Name = "Street")]
        [Required]
        public string Street { get; set; }

        [Display(Name = "Barangay")]
        [Required]
        public string Baranggay { get; set; }

        [Display(Name = "Profile Picture")]
        [Required]
        public byte[] ProfilePicture { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }

    }
}