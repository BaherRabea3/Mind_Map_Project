using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MindMapManager.Core.DTOs
{
    public class UpdateTrackRequestDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? TrackName { get; set; }

        [MaxLength(500)]
        public string? TrackDescription { get; set; }

        public IFormFile? TrackImage { get; set; }
    }

}
