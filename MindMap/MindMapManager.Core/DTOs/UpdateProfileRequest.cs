using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class UpdateProfileRequest
    {

        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression(
              @"^[a-zA-Z\s'.-]+$",
              ErrorMessage = "Full name may contain letters and spaces only"
          )]
        public string? FullName { get; set; }

    }
}
