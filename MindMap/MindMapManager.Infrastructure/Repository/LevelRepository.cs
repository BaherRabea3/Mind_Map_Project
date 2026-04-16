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

        public bool IsExist(Level level)
        {
            return _context.Levels.Any(l => l.Name == level.Name && l.Rid == level.Rid);
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
    public class ResourceRepository : IResourceRepository
    {
        private readonly AppDbContext _context;

        public ResourceRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Resource resource)
        {
            _context.Add(resource);
        }

        public void Delete(Resource resource)
        {
            _context.Remove(resource);
        }

        public Resource? GetById(int id)
        {
            return _context.Resources.FirstOrDefault(x => x.ResId == id);
        }

        public bool IsExist(Resource resource)
        {
            return _context.Resources.Any(l => l.Name == resource.Name && l.ResUrl == resource.ResUrl);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Resource resource)
        {
            _context.Update(resource);
        }
    }
}
