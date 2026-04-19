
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public Review? GetById(int id)
        {
            return _context.Reviews.FirstOrDefault(r => r.RevId == id);
        }

        public List<Review> GetByRoadmapId(int roadmapId)
        {
            return _context.Reviews
                .Where(r => r.Rid == roadmapId)
                .ToList();
        }

        public Review? GetByUserAndRoadmap(int userId, int roadmapId)
        {
            return _context.Reviews
                .FirstOrDefault(r => r.UserId == userId && r.Rid == roadmapId);
        }

        public void Add(Review review)
        {
            _context.Add(review);
        }

        public void Delete(Review review)
        {
            _context.Remove(review);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}