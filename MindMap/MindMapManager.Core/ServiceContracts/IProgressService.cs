using MindMapManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IProgressService
    {
        void CompleteTopic(int userId, int topicId);
        RoadmapProgressResponse RoadmapProgress(int userId ,int roadmapId);
    }
}
