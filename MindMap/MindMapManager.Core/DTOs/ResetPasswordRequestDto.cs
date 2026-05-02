using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class ResetPasswordRequestDto
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string ResetToken { get; set; } = string.Empty ;

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirm password must be match")]
        [Required]
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}
