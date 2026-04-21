
using MindMapManager.Core.DTOs;

using MindMapManager.Core.Helpers;
namespace MindMapManager.Core.ServiceContracts

{
    public interface IAdminService
    {
        public PagedResult<UserResponse> GetAllUsers(UserFilterRequest filter);
        public UserResponse GetUserById(int id);
        public void ChangeUserStatus(int id, string status);
        public void ChangeUserRole(int id, string role);
        public void DeleteUser(int id);
        public DashboardResponse GetDashboard();
    }
}