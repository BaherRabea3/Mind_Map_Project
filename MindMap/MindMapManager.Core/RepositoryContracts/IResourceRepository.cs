using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IResourceRepository
    {
        public void Add(Resource level);
        public void Update(Resource level);
        public Resource? GetById(int id);
        public void Delete(Resource level);
        public void Save();
        public bool IsExist(string newName, string newUrl, int topicId, int? excludeId);
    }
}
