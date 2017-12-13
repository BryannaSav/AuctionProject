using System.ComponentModel.DataAnnotations;

namespace AuctionProject.Models
{
    public class UserViewModel
    {
        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$")]
        [Display(Name="First Name: ")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression("^[a-zA-Z]+$")]
        [Display(Name="Last Name: ")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        [MinLength(3)]
        [MaxLength(20)]
        [Display(Name="Username: ")]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        [Display(Name="Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [Display(Name="Confirm Password: ")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}