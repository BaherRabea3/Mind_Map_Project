
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Helpers;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IBookmarkService
    {
        public void AddBookmark(int userId, int resourceId);
        public void RemoveBookmark(int userId, int resourceId);
        public PagedResult<BookmarkResponse> GetMyBookmarks(int userId, int page, int pageSize);
    }
}