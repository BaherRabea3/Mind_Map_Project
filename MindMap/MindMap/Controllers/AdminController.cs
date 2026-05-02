using MindMapManager.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using System.Threading.Tasks;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : CustomControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        
        [HttpGet("users")]
        public ActionResult GetAllUsers([FromQuery] UserFilterRequest filter)
        {
            var users = _adminService.GetAllUsers(filter);
            return Ok(users);
        }

       
        [HttpGet("users/{id:int}")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _adminService.GetUserDetails(id);
            return Ok(user);
        }

       
        [HttpPut("users/{id:int}/status")]
        public async Task<ActionResult> ChangeStatus(int id, ChangeStatusRequest request)
        {
           await _adminService.ChangeUserStatus(id, request.Status);
           return NoContent();
        }

        
        [HttpPut("users/{id:int}/role")]
        public async Task<ActionResult> ChangeRole(int id, ChangeRoleRequest request)
        {
            await _adminService.ChangeUserRole(id, request.Role);
            return NoContent();
        }

      
        [HttpDelete("users/{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await  _adminService.DeleteUser(id);
             return NoContent();
        }

       
        [HttpGet("dashboard")]
        public ActionResult GetDashboard()
        {
            var dashboard = _adminService.GetDashboard();
            return Ok(dashboard);
        }
    }
}