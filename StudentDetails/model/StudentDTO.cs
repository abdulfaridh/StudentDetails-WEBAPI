using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace StudentDetails.model
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Student name is required")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "EmailAddess is a required field")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "phone number is required")]
        public string PhoneNumber { get; set; }
     
        public string Address { get; set; }
        public DateTime DOB { get; set; }
    }
}
