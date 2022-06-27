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

        //Foreign Keys
        public int TypeID { get; set; }

        public List<UserTypeModel> Types { get; set; } //UI Rendering

        public string Type { get; set; }

        //Details

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ]*)+", ErrorMessage = "Invalid Name")]
        [MinLength(2, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too long")]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ]*)+", ErrorMessage = "Invalid Name")]
        [MinLength(2, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too long")]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [RegularExpression(@"^09(73|74|05|06|15|16|17|26|27|35|36|37|79|38|07|08|09|10|12|18|19|20|21|28|29|30|38|39|89|99|22|23|32|33)\d{3}\s?\d{4}", ErrorMessage = "Invalid Mobile No.")]
        [MaxLength(30)]
        [Required]
        public string MobileNo { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid format.")]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.{4,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$", ErrorMessage = "Invalid Username, use 4-20 characters, and NO special characters")]
        [System.Web.Mvc.Remote(action: "VerifyUsername", controller: "UserController")]
        [Display(Name = "Username")]
        [Required]
        public string UserName { get; set; } = null;

        //min 8 char, 1 uppercase, 1 lowercase, 1 number, 1 special char
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum 8 characters, and at least: 1 uppercase, 1 lowercase, and 1 special character")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string Gender { get; set; }

        //edit here to make birthday for 18-120 year olds
        [Required]
        [CustomDate(ErrorMessage = "You must be 18+ to use this website")]
        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        //only accepts numbers and letters only
        [RegularExpression(@"^[A-Za-z0-9_-]*$", ErrorMessage = "Invalid House No.")]
        [Display(Name = "Unit House No.")]
        [Required]
        public string UnitHouseNo { get; set; }

        [RegularExpression(@"^[0-9a-zA-Z]{0,20}", ErrorMessage = "Invalid Street Name")]
        [Display(Name = "Street")]
        [Required]
        public string Street { get; set; }

        [RegularExpression(@"^\b([A-ZÀ-ÿ][-,a-z. ']+[ ]*)+", ErrorMessage = "Invalid Bargangay Name")]
        [MinLength(3, ErrorMessage = "Name is too short")]
        [MaxLength(25, ErrorMessage = "Name is too short")]
        [Display(Name = "Barangay")]
        [Required]
        public string Baranggay { get; set; }

        [Display(Name = "Profile Picture")]
        [Required]
        public string ProfilePicture { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }

    }
    public class CustomDateAttribute : RangeAttribute
    {
        public CustomDateAttribute()
          : base(typeof(DateTime),
                  DateTime.Now.AddYears(-120).ToShortDateString(),
                  DateTime.Now.AddYears(-18).ToShortDateString())
        { }
    }
}