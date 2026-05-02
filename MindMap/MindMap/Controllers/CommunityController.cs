
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

        
        [HttpGet("/api/Topic/{id:int}/comments")]
        public ActionResult GetTopicComments(int id)
        {
            var comments = _commentService.GetTopicComments(id, GetUserId());
            return Ok(comments);
        }

     
        [HttpPost("/api/Topic/{id:int}/comments")]
        public ActionResult AddComment(int id, CommunityRequest request)
        {
            _commentService.AddComment(id, GetUserId(), request);
            return StatusCode(StatusCodes.Status201Created);
        }

     
        [HttpPost("/api/comments/{id:int}/reply")]
        public ActionResult ReplyToComment(int id, CommunityRequest request)
        {
            _commentService.ReplyToComment(id, GetUserId(), request);
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpDelete("/api/comments/{id:int}")]
        public ActionResult DeleteComment(int id)
        {
            _commentService.DeleteComment(id, GetUserId(), IsAdmin());
            return NoContent();
        }
    }
}