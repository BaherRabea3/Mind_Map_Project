using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("progress")]
        public ActionResult GetProgress()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var response = _usersService.GetUserProgress(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
