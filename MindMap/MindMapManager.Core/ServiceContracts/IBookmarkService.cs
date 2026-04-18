
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IBookmarkService
    {
        public void AddBookmark(int userId, int resourceId);
        public void RemoveBookmark(int userId, int resourceId);
        public List<BookmarkResponse> GetMyBookmarks(int userId);
    }
}