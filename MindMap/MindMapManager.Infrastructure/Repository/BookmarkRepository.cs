
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MindMapManager.Infrastructure.Repository
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly AppDbContext _context;

        public BookmarkRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsBookmarked(int userId, int resourceId)
        {
            var user = _context.Users
                .Include(u => u.Res)
                .FirstOrDefault(u => u.Id == userId);

            return user?.Res.Any(r => r.ResId == resourceId) ?? false;
        }

        public void Add(int userId, int resourceId)
        {
            var user = _context.Users
                .Include(u => u.Res)
                .FirstOrDefault(u => u.Id == userId);

            var resource = _context.Resources.Find(resourceId);

            if (user == null || resource == null) return;

            user.Res.Add(resource);
            _context.SaveChanges();
        }

        public void Remove(int userId, int resourceId)
        {
            var user = _context.Users
                .Include(u => u.Res)
                .FirstOrDefault(u => u.Id == userId);

            var resource = user?.Res.FirstOrDefault(r => r.ResId == resourceId);

            if (user == null || resource == null) return;

            user.Res.Remove(resource);
            _context.SaveChanges();
        }

        public List<Resource> GetUserBookmarks(int userId)
        {
            var user = _context.Users
                .Include(u => u.Res)
                .FirstOrDefault(u => u.Id == userId);

            return user?.Res.ToList() ?? new List<Resource>();
        }
    }
}