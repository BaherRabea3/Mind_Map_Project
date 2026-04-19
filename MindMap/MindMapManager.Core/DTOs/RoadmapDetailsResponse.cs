using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class RoadmapDetailsResponse
    {
        public int roadmapId { get; set; }
        public string roadmapName {  get; set; }
        public string? trackName { get; set; }
        public string roadmapDescription { get; set; }
        public double avarageRating { get; set; }
        public IEnumerable<LevelResoponse> levelResoponses { get; set; }
    }
}
