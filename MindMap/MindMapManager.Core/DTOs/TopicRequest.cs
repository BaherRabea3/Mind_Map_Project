using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class TopicRequest
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Name length must be between 2 and 100 characters")]
        public string name {  get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [Range(1,100,ErrorMessage = "{0} is invalid")]
        public int Order {  get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        public int levelId { get; set; }
    }
}
