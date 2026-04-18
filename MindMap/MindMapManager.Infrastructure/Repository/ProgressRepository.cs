
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MindMapManager.Infrastructure.Repository
{
    public class ProgressRepository : IProgressRepository
    {
        private readonly AppDbContext _context;

        public ProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsRoadmapCompleted(int userId, int roadmapId)
        {
            var totalLevels = _context.Levels
                .Where(l => l.Rid == roadmapId)
                .Count();

            if (totalLevels == 0) return false;

            var completedLevels = _context.Progresses
                .Include(p => p.LidNavigation)
                .Where(p => p.UserId == userId
                    && p.LidNavigation != null
                    && p.LidNavigation.Rid == roadmapId
                    && p.CompPerc == 100)
                .Count();

            return completedLevels >= totalLevels;
        }
    }
}