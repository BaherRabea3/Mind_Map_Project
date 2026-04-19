
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IBookmarkRepository
    {
        public bool IsBookmarked(int userId, int resourceId);
        public void Add(int userId, int resourceId);
        public void Remove(int userId, int resourceId);
        public List<Resource> GetUserBookmarks(int userId);
    }
}