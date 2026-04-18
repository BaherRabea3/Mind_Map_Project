
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IProgressRepository
    {
        public bool IsRoadmapCompleted(int userId, int roadmapId);
    }
}