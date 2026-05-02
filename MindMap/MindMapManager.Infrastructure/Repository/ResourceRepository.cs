using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
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

        public bool IsExist(string newName , string newUrl ,int topicId, int? execludeId)
        {
            return _context.Resources
                .Any(l => l.Name == newName 
                && l.ResUrl == newUrl 
                && l.TopicId == topicId
                && !execludeId.HasValue || l.ResId != execludeId);
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
