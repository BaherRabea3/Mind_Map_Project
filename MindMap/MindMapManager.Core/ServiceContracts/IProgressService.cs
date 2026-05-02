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
        Task CompleteTopic(int userId, int topicId);
        void UnCompleteTopic(int userId, int topicId);
        Task<RoadmapProgressResponse> RoadmapProgress(int userId ,int roadmapId);
    }
}
