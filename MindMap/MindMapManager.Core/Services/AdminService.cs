
using Microsoft.AspNetCore.Identity;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
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

        private async Task<ApplicationUser?> GetUserById(int id) =>
            await _userManager.FindByIdAsync(id.ToString());
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

        public async Task<UserResponse> GetUserDetails(int id)
        {
            var user = await GetUserById(id);
            if (user == null) throw new NotFoundException("user not found");
            return MapToResponse(user);
        }

        public async Task ChangeUserStatus(int id, string status)
        {
            var user = await GetUserById(id);
            if (user == null) throw new NotFoundException("user not found");

            user.Status = status;
            
            await _userManager.UpdateAsync(user);
        }

        public async Task ChangeUserRole(int id, string role)
        {
            var user = await GetUserById(id);
            if (user == null) throw new NotFoundException("user not found");

            var currentRoles = await _userManager.GetRolesAsync(user);
           await  _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task DeleteUser(int id)
        {
            var user = await GetUserById(id);
            if (user == null) throw new NotFoundException("user not found");

            await _userManager.DeleteAsync(user);
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