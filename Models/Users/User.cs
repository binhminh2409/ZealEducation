using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ZealEducation.Models.Faculty
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set;}

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

    }
}
