using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionProject.Models
{
    public class AuctionViewModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [Range(0,9999999999999999999)]
        public float StartingBid { get; set; }

        [Required]
        [FutureDate]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

    }

    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            if(date < DateTime.Now){
               return new ValidationResult("Invalid Date");
            }
            else{
                return ValidationResult.Success;
            }
        }
    }
}