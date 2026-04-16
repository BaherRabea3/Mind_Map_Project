using System.ComponentModel.DataAnnotations;

namespace MindMapManager.Core.DTOs
{
    public class LevelRequest
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 100 characters")]
        public string name { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        public int roadmapId { get; set; }
    }
}
