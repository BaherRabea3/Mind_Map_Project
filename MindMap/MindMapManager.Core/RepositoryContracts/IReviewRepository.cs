
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IReviewRepository
    {
        public Review? GetById(int id);
        public IQueryable<Review> GetByRoadmapId(int roadmapId);
        public Review? GetByUserAndRoadmap(int userId, int roadmapId);
        public void Add(Review review);
        public void Delete(Review review);
        public void Save();
    }
}