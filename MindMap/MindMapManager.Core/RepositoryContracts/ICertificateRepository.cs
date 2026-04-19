
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ICertificateRepository
    {
        public Certificate? GetById(int id);
        public List<Certificate> GetByUserId(int userId);
        public Certificate? GetByUserAndRoadmap(int userId, int roadmapId);
        public void Add(Certificate certificate);
        public void Save();
    }
}
