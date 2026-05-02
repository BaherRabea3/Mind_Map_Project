
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

       
        [HttpGet("/api/Roadmap/{id:int}/reviews")]
        public ActionResult GetRoadmapReviews(
            int id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var reviews = _reviewService.GetRoadmapReviews(id, page, pageSize);
            return Ok(reviews);
        }


        [HttpPost("/api/Roadmap/{id:int}/reviews")]
        [Authorize]
        public ActionResult AddReview(int id, ReviewRequest request)
        {
            _reviewService.AddReview(GetUserId(), id, request);
            return StatusCode(StatusCodes.Status201Created);
        }


        [HttpDelete("/api/reviews/{id:int}")]
        [Authorize]
        public ActionResult DeleteReview(int id)
        {
            _reviewService.DeleteReview(id, GetUserId(), IsAdmin());
            return NoContent();
        }
    }
}