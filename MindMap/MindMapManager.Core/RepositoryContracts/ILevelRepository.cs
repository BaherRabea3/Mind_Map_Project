using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ILevelRepository
    {
        public void Add(Level level);
        public void Update(Level level);
        public Level? GetById(int id);
        public Level? GetByidWithDetails(int id);
        public void Delete(Level level);
        public void Save();
        public bool IsExist(Level level);
    }
}
