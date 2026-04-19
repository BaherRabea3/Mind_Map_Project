using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.Entities
{
    public class UserTrack
    {
        public int userId {  get; set; }
        public int trackId { get; set; }
        public DateTime? EnrolledAt { get; set; }
        
        public ApplicationUser User { get; set; }
        public Track Track { get; set; }

    }
}
