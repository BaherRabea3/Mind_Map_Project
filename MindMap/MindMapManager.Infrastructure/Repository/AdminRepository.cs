using MindMapManager.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public PagedResult<ApplicationUser> GetAllUsers(string? status, string? search, int page, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(u => u.Status == status);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(u =>
                    (u.UserName != null && u.UserName.Contains(search)) ||
                    (u.Email != null && u.Email.Contains(search)));

            query = query.OrderByDescending(u => u.CreatedAt);

            return new PagedResult<ApplicationUser>
            {
                Items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                TotalCount = query.Count(),
                Page = page,
                PageSize = pageSize
            };
        }

        public ApplicationUser? GetUserById(int id)
        {
            return _context.Users
                .Include(u => u.Tracks)
                .Include(u => u.Certificates)
                .FirstOrDefault(u => u.Id == id);
        }

        public void UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
        }

        public void DeleteUser(ApplicationUser user)
        {
            _context.Remove(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public DashboardData GetDashboardData()
        {
            var now = DateTime.UtcNow;
            var weekAgo = now.AddDays(-7);
            var monthAgo = now.AddDays(-30);

            return new DashboardData
            {
                TotalUsers = _context.Users.Count(),
                ActiveUsers = _context.Users.Count(u => u.LastActDate >= monthAgo),
                TotalTracks = _context.Tracks.Count(),
                TotalRoadmaps = _context.Roadmaps.Count(),
                TotalEnrollments = _context.Users.SelectMany(u => u.Tracks).Count(),
                TotalCertificates = _context.Certificates.Count(),
                TotalComments = _context.Comments.Count(),
                NewUsersThisWeek = _context.Users.Count(u => u.CreatedAt >= weekAgo),
                NewUsersThisMonth = _context.Users.Count(u => u.CreatedAt >= monthAgo)
            };
        }
    }
}