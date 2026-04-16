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
            if (!ModelState.IsValid)
            {
                string msg = string.Join(" | ",
                    ModelState.Values.SelectMany(v => v.Errors)
                    .Select(error => error.ErrorMessage));
                return BadRequest(msg);
            }
            try
            {
                var response = _roadmapService.GetAll(page, pageSize, searchTirm);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(RoadmapRequestDto roadmapDto)
        {
            try
            {
                var roadmapResponse = _roadmapService.AddRoadmap(roadmapDto);
                return CreatedAtAction("GetRoadmapById", "roadmap", new { id = roadmapResponse.id }, roadmapResponse);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("internal"))
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = ("Admin"))]
        public ActionResult RemoveRoadmap(int id)
        {
            try
            {
                _roadmapService.DeleteRoadmap(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Failed"))
                {
                    return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
                    
                return NotFound();
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, RoadmapRequestDto roadmapRequest)
        {
            try
            {
                _roadmapService.UpdateRoadmap(id, roadmapRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if(ex.Message.Contains("Failed"))
                    return Problem(ex.Message , statusCode: StatusCodes.Status500InternalServerError);

                return BadRequest(ex.Message);
            }
        }
    }
}
