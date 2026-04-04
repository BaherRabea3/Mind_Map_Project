using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MindMapManager.Core.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name can't be blank")]
        [MinLength(3)]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z0-9_.\\ -]+$")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be a proper email address")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already taken")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password can't be blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password", ErrorMessage = "Confirm password must match with Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
