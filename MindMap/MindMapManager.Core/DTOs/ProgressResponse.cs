using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class RoadmapProgressResponse
    {
        public int roadmapId {  get; set; }
        public string roadmapName { get; set; }
        public string roadmapDescription { get; set; }
        public string lastTopicCompleted { get; set; }
        public int Percentage { get; set; }
        public IEnumerable<LevelProgressResponse> Levels { get; set; }
    }
}
