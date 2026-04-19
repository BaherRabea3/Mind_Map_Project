using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class ResourceResponse
    {
        public int resourceId {  get; set; }
        public string resourceName {  get; set; }
        public string? resourceType { get; set; }
        public int resourceOrder { get; set; }
        public bool? IsPaid { get; set; }
        public string rsourceUrl { get; set; }
    }
}
