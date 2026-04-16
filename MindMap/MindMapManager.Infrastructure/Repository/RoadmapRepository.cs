using Microsoft.EntityFrameworkCore;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class RoadmapRepository : IRoadmapRepository
    {
        private readonly AppDbContext _context;

        public RoadmapRepository(AppDbContext context)
        {
           _context = context;
        }
        public void Add(Roadmap roadmap)
        {
            _context.Roadmaps.Add(roadmap);
        }

        public IQueryable<Roadmap> FilterByTrackId(int trackId)
        {
            return _context.Roadmaps.Where(r => r.TrackId == trackId);
        }

        public Roadmap? FindByName(string roadmapName)
        {
            return _context.Roadmaps.FirstOrDefault(r => r.Name == roadmapName);
        }

        public IQueryable<Roadmap> Get()
        {
            return _context.Roadmaps.AsQueryable();
        }

        public Roadmap? GetById(int id)
        {
            return _context.Roadmaps.FirstOrDefault(r => r.Rid == id);
        }

        public Roadmap? GetByIdWithDetails(int id)
        {
            return _context.Roadmaps
                .Include(r => r.Levels)
                    .ThenInclude(l => l.Topics)
                        .ThenInclude(t => t.Resources)
                .Include(r => r.Reviews)
                .Include(r => r.Track)
                .FirstOrDefault(r => r.Rid == id);
        }

        public bool IsExist(Roadmap roadmap)
        {
            return _context.Roadmaps.Any(r => 
            r.Name == roadmap.Name && r.TrackId == roadmap.TrackId);
        }

        public void Remove(Roadmap roadmap)
        {
            _context.Remove(roadmap);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public IQueryable<Roadmap> Search(string? searchTirm)
        {
            if (string.IsNullOrWhiteSpace(searchTirm))
                return Get();
            string key = searchTirm.ToLower().Trim();

            bool result = Get().Any(r => r.Name.Contains(key) || 
            r.Description.Contains(key));

            if(!result)
                return Get();

            return Get().Where(r => r.Name.Contains(key) ||
            r.Description.Contains(key));

        }

        public void Update(Roadmap roadmap)
        {
            _context.Update(roadmap);
        }
    }
}
