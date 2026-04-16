using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class LevelResoponse
    {
        public string levelName { get; set; }
        public IEnumerable<TopicResponse> topicResponses { get; set; }
    }
}
