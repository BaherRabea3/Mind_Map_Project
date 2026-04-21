using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class enrollResponseDto
    {
        public int trackId {  get; set; }
        public string trackName { get; set; }
        public DateTime EnrolledAt { get; set; }
        public string enrolledMessage { get; set; }
    }
}
