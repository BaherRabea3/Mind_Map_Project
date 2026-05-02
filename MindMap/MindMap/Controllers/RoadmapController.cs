using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.WebAPI.Controllers
{
    public class RoadmapController : CustomControllerBase
    {
        private readonly IRoadmapService _roadmapService;

        public RoadmapController(IRoadmapService roadmapService)
        {
            _roadmapService = roadmapService;
        }

        [HttpGet("{id:int}")]
        public ActionResult GetRoadmapById([FromRoute] int id)
        {
            try
            {
                var response = _roadmapService.GetRoadmapDetails(id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult GetAllRoadmaps(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? searchTirm = null)
        {
            var response = _roadmapService.GetAll(page, pageSize, searchTirm);

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(RoadmapRequestDto roadmapDto)
        {
            var roadmapResponse = _roadmapService.AddRoadmap(roadmapDto);
            return CreatedAtAction("GetRoadmapById", "roadmap", new { id = roadmapResponse.id }, roadmapResponse);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = ("Admin"))]
        public ActionResult RemoveRoadmap(int id)
        {
            _roadmapService.DeleteRoadmap(id);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, RoadmapRequestDto roadmapRequest)
        {
            _roadmapService.UpdateRoadmap(id, roadmapRequest);
            return NoContent();
        }
    }
}
