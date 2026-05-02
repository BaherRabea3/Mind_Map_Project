
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ICertificateRepository
    {
        public Certificate? GetById(int id, int userId);
        public Certificate? GetByCodeWithUserAndRoadmap(string code);
        public List<Certificate> GetByUserIdWithRoadmaps(int userId);
        public Certificate? GetByUserAndRoadmap(int userId, int roadmapId);
        bool IsAlreadyExisted(int userId ,  int roadmapId);
        public void Add(Certificate certificate);
        public void Save();
    }
}
