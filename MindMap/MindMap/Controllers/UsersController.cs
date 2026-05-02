using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize(Roles = "Member")]
    public class UsersController : CustomControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        private int GetUser() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("profile")]
        public async Task<ActionResult> GetProfile()
        {
            var response = await _usersService.GetUserDetails(GetUser());
            return Ok(response);
        }

        [HttpPut("profile")]
        public async Task<ActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            await _usersService.UpdateProfile(GetUser(), request);
            return NoContent();
        }

        [HttpGet("progress")]
        public ActionResult GetProgress()
        {
            var response = _usersService.GetUserProgress(GetUser());
            return Ok(response);
        }
    }
}
