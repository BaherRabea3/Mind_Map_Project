using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.Entities
{
    public class CompletedTopic
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int topicId { get; set; }

        public ApplicationUser User { get; set; }
        public Topic Topic { get; set; }
    }
}
