using MindMapManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class TrackRequestDto
    {
        [Required(ErrorMessage = "*")]
        [StringLength(maximumLength: 100 , MinimumLength = 2 , ErrorMessage = "Name length must be between 2 and 100")]
        public string TrackName { get; set; }
        [Required(ErrorMessage = "*")]
        [MaxLength(500, ErrorMessage = "Description length must not exceed 500")]
        public string TrackDescription { get; set; }
        [Required(ErrorMessage = "*")]
        public string TrackImage {  get; set; }

    }
}
