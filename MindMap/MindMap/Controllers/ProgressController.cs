using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    public class ProgressController : CustomControllerBase
    {
        private readonly IProgressService _progressService;

        public ProgressController(IProgressService progressService)
        {
            _progressService = progressService;
        }

        [Authorize(Roles = "Member")]
        [HttpGet("roadmap/{roadmapId:int}")]
        public ActionResult GetRoadmapProgress([FromRoute] int roadmapId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var progressResponse = _progressService.RoadmapProgress(userId, roadmapId);
            return Ok(progressResponse);
        }

        [Authorize(Roles = "Member")]
        [HttpPost("complete-topic/{topicId:int}")]
        public async Task<ActionResult> CompleteTopic([FromRoute] int topicId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            await _progressService.CompleteTopic(userId, topicId);
            return Created();
        }

        [Authorize(Roles = "Member")]
        [HttpDelete("complete-topic/{topicId:int}")]
        public async Task<ActionResult> UnCompleteTopic([FromRoute] int topicId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            _progressService.UnCompleteTopic(userId, topicId);
            return NoContent();
        }
    }
}
