
using MindMapManager.Core.DTOs;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepo;

        public BookmarkService(IBookmarkRepository bookmarkRepo)
        {
            _bookmarkRepo = bookmarkRepo;
        }

        public void AddBookmark(int userId, int resourceId)
        {
            if (_bookmarkRepo.IsBookmarked(userId, resourceId))
                throw new Exception("already bookmarked");

            _bookmarkRepo.Add(userId, resourceId);
        }

        public void RemoveBookmark(int userId, int resourceId)
        {
            if (!_bookmarkRepo.IsBookmarked(userId, resourceId))
                throw new Exception("not found");

            _bookmarkRepo.Remove(userId, resourceId);
        }

        public List<BookmarkResponse> GetMyBookmarks(int userId)
        {
            var resources = _bookmarkRepo.GetUserBookmarks(userId);
            return resources.Select(r => new BookmarkResponse
            {
                ResId = r.ResId,
                Name = r.Name,
                Type = r.Type,
                ResUrl = r.ResUrl,
                Paid = r.Paid
            }).ToList();
        }
    }
}