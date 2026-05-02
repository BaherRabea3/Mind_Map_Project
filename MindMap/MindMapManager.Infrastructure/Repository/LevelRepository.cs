using Microsoft.EntityFrameworkCore;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class LevelRepository : ILevelRepository
    {
        private readonly AppDbContext _context;

        public LevelRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Level level)
        {
            _context.Add(level);
        }

        public void Delete(Level level)
        {
            _context.Remove(level);
        }

        public Level? GetById(int id)
        {
            return _context.Levels.FirstOrDefault(x => x.Lid == id);
        }

        public Level? GetByidWithDetails(int id)
        {
            return _context.Levels
                .Include(l => l.Topics)
                    .ThenInclude(t => t.Resources)
                .FirstOrDefault(l => l.Lid == id);

        }

        public bool IsExist(string newName , int roadmapId , int levelId)
        {
            return _context.Levels.Any(l => l.Name == newName && l.Rid == roadmapId && l.Lid != levelId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Level level)
        {
            _context.Update(level);
        }
    }
}
