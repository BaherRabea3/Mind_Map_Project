
namespace MindMapManager.Core.DTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? Streak { get; set; }
        public DateTime? LastActDate { get; set; }
    }

    public class UserFilterRequest
    {
        public string? Status { get; set; }
        public string? Role { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class ChangeStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }

    public class ChangeRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }

    public class DashboardResponse
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