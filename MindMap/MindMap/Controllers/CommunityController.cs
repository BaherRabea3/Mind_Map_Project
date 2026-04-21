
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize]
    public class CommunityController : CustomControllerBase
    {
        private readonly ICommunityService _commentService;

        public CommunityController(ICommunityService commentService)
        {
            _commentService = commentService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin() => User.IsInRole("Admin");

        
        [HttpGet("/api/topics/{id:int}/comments")]
        public ActionResult GetTopicComments(int id)
        {
            try
            {
                var comments = _commentService.GetTopicComments(id, GetUserId());
                return Ok(comments);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("forbidden"))
                    return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
                return BadRequest(ex.Message);
            }
        }

     
        [HttpPost("/api/topics/{id:int}/comments")]
        public ActionResult AddComment(int id, CommunityRequest request)
        {
            try
            {
                _commentService.AddComment(id, GetUserId(), request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("forbidden"))
                    return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }

     
        [HttpPost("/api/comments/{id:int}/reply")]
        public ActionResult ReplyToComment(int id, CommunityRequest request)
        {
            try
            {
                _commentService.ReplyToComment(id, GetUserId(), request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if (ex.Message.Contains("forbidden"))
                    return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("/api/comments/{id:int}")]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                _commentService.DeleteComment(id, GetUserId(), IsAdmin());
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if (ex.Message.Contains("forbidden"))
                    return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }

        
        [HttpDelete("/api/admin/comments/{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDeleteComment(int id)
        {
            try
            {
                _commentService.DeleteComment(id, GetUserId(), true);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                return BadRequest(ex.Message);
            }
        }
    }
}