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

            try
            {
                var progressResponse = _progressService.RoadmapProgress(userId, roadmapId);
                return Ok(progressResponse);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Member")]
        [HttpPost("complete-topic/{topicId:int}")]
        public ActionResult CompleteTopic([FromRoute] int topicId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                _progressService.CompleteTopic(userId, topicId);
                return Created();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
