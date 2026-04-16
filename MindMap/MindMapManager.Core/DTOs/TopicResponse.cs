using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class TopicResponse
    {
        public string topicName {  get; set; }
        public int topicOrder { get; set; }

        public IEnumerable<ResourceResponse> resources { get; set; }
    }
}
