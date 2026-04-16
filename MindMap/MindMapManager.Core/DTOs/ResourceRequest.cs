using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class ResourceRequest
    {
        public string resourceName { get; set; }
        public string? resourceType { get; set; }
        public int resourceOrder { get; set; }
        public bool? IsPaid { get; set; }
        [Url(ErrorMessage = "{0} must be a proper url")]
        public string rsourceUrl { get; set; }
        public int topicId { get; set; }
    }
}
