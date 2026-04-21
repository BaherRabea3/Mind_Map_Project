using MindMapManager.Core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;

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

        
        [HttpGet("/api/admin/users")]
        public ActionResult GetAllUsers([FromQuery] UserFilterRequest filter)
        {
            var users = _adminService.GetAllUsers(filter);
            return Ok(users);
        }

       
        [HttpGet("/api/admin/users/{id:int}")]
        public ActionResult GetUserById(int id)
        {
            try
            {
                var user = _adminService.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found")) return NotFound();
                return BadRequest(ex.Message);
            }
        }

       
        [HttpPut("/api/admin/users/{id:int}/status")]
        public ActionResult ChangeStatus(int id, ChangeStatusRequest request)
        {
            try
            {
                _adminService.ChangeUserStatus(id, request.Status);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found")) return NotFound();
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("/api/admin/users/{id:int}/role")]
        public ActionResult ChangeRole(int id, ChangeRoleRequest request)
        {
            try
            {
                _adminService.ChangeUserRole(id, request.Role);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found")) return NotFound();
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete("/api/admin/users/{id:int}")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                _adminService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found")) return NotFound();
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }

       
        [HttpGet("/api/admin/dashboard")]
        public ActionResult GetDashboard()
        {
            var dashboard = _adminService.GetDashboard();
            return Ok(dashboard);
        }
    }
}