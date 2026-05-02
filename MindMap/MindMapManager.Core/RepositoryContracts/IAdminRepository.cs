using MindMapManager.Core.Helpers;
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IAdminRepository
    {
        public PagedResult<ApplicationUser> GetAllUsers(string? status, string? search, int page, int pageSize);
        public void Save();
        public DashboardData GetDashboardData();
    }

    public class DashboardData
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalTracks { get; set; }
        public int TotalRoadmaps { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalCertificates { get; set; }
        public int TotalComments { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int NewUsersThisMonth { get; set; }
    }
}