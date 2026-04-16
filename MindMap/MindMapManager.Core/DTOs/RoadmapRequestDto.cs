using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class RoadmapRequestDto
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 100 characters")]
        public string RoadmapName { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [MaxLength(500, ErrorMessage = "Description length must not exceed 500 characters")]
        public string RoadmapDescription { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        public int trackId { get; set; }
    }
}
