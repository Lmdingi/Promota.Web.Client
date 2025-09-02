using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "First Name cannot contain spaces.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "Last Name cannot contain spaces.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Email must not have leading or trailing spaces.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^\S(.*\S)?$", ErrorMessage = "Password must not have leading or trailing spaces.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "Username cannot contain spaces.")]
        [Display(Name = "Username")]
        public string UserName { get; set; } = string.Empty;

        [MustBeTrue(ErrorMessage = "You must accept the terms.")]
        public bool AcceptTerms { get; set; }
    }

    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool boolValue && boolValue;
        }
    }
}
