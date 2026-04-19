
using Microsoft.AspNetCore.Identity;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(IAdminRepository adminRepo, UserManager<ApplicationUser> userManager)
        {
            _adminRepo = adminRepo;
            _userManager = userManager;
        }

        public PagedResult<UserResponse> GetAllUsers(UserFilterRequest filter)
        {
            var result = _adminRepo.GetAllUsers(filter.Status, filter.Search, filter.Page, filter.PageSize);
            return new PagedResult<UserResponse>
            {
                Items = result.Items.Select(MapToResponse).ToList(),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }

        public UserResponse GetUserById(int id)
        {
            var user = _adminRepo.GetUserById(id);
            if (user == null) throw new Exception("not found");
            return MapToResponse(user);
        }

        public void ChangeUserStatus(int id, string status)
        {
            var user = _adminRepo.GetUserById(id);
            if (user == null) throw new Exception("not found");

            user.Status = status;
            _adminRepo.UpdateUser(user);
            try { _adminRepo.Save(); }
            catch (Exception ex) { throw new Exception($"Failed : {ex.Message}"); }
        }

        public void ChangeUserRole(int id, string role)
        {
            var user = _adminRepo.GetUserById(id);
            if (user == null) throw new Exception("not found");

            var currentRoles = _userManager.GetRolesAsync(user).Result;
            _userManager.RemoveFromRolesAsync(user, currentRoles).Wait();
            _userManager.AddToRoleAsync(user, role).Wait();
        }

        public void DeleteUser(int id)
        {
            var user = _adminRepo.GetUserById(id);
            if (user == null) throw new Exception("not found");

            _adminRepo.DeleteUser(user);
            try { _adminRepo.Save(); }
            catch (Exception ex) { throw new Exception($"Failed : {ex.Message}"); }
        }

        public DashboardResponse GetDashboard()
        {
            var data = _adminRepo.GetDashboardData();
            return new DashboardResponse
            {
                TotalUsers = data.TotalUsers,
                ActiveUsers = data.ActiveUsers,
                TotalTracks = data.TotalTracks,
                TotalRoadmaps = data.TotalRoadmaps,
                TotalEnrollments = data.TotalEnrollments,
                TotalCertificates = data.TotalCertificates,
                TotalComments = data.TotalComments,
                NewUsersThisWeek = data.NewUsersThisWeek,
                NewUsersThisMonth = data.NewUsersThisMonth
            };
        }

        private static UserResponse MapToResponse(ApplicationUser u) => new()
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Status = u.Status,
            CreatedAt = u.CreatedAt,
            Streak = u.Streak,
            LastActDate = u.LastActDate
        };
    }
}