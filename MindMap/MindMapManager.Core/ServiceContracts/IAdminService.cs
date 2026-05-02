
using MindMapManager.Core.DTOs;

using MindMapManager.Core.Helpers;
namespace MindMapManager.Core.ServiceContracts

{
    public interface IAdminService
    {
        public PagedResult<UserResponse> GetAllUsers(UserFilterRequest filter);
        public Task<UserResponse> GetUserDetails(int id);
        public Task ChangeUserStatus(int id, string status);
        public Task ChangeUserRole(int id, string role);
        public Task DeleteUser(int id);
        public DashboardResponse GetDashboard();
    }
}