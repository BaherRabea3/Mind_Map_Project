
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    public class ReviewsController : CustomControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private bool IsAdmin() =>
            User.IsInRole("Admin");

       
        [HttpGet("/api/roadmaps/{id:int}/reviews")]
        public ActionResult GetRoadmapReviews(int id)
        {
            var reviews = _reviewService.GetRoadmapReviews(id);
            return Ok(reviews);
        }

       
        [HttpPost("/api/roadmaps/{id:int}/reviews")]
        [Authorize]
        public ActionResult AddReview(int id, ReviewRequest request)
        {
            try
            {
                _reviewService.AddReview(GetUserId(), id, request);
                return StatusCode(StatusCodes.Status201Created);
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

        
        [HttpDelete("/api/reviews/{id:int}")]
        [Authorize]
        public ActionResult DeleteReview(int id)
        {
            try
            {
                _reviewService.DeleteReview(id, GetUserId(), IsAdmin());
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if (ex.Message.Contains("forbidden"))
                    return Forbid();
                if (ex.Message.Contains("Failed"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);

                return BadRequest(ex.Message);
            }
        }
    }
}