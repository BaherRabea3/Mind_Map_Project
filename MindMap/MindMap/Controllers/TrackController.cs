using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.WebAPI.Controllers
{
    public class TrackController : CustomControllerBase
    {
        private readonly ITrackService _trackService;

        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
        }



        [HttpGet]
        public ActionResult GetAllTracks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6,
            [FromQuery] string? searchTirm = null)
        {
            try
            {
                var Response = _trackService.GetAll(page, pageSize, searchTirm);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }

        }

        [HttpGet("Featured-tracks")]
        public IActionResult FeaturedTracks([FromQuery] int amount = 4)
        {
            try
            {
                var Response = _trackService.GetTracksWithMostEnrollments(amount);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }


        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(TrackRequestDto TrackRequest)
        {
            if (!ModelState.IsValid)
            {
                var msg = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
                return Problem(msg, statusCode: StatusCodes.Status400BadRequest);
            }
            try
            {
                _trackService.AddTrack(TrackRequest);
                return Created();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _trackService.DeleteTrack(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "not found")
                    return NotFound();
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id , TrackRequestDto trackRequest)
        {
            try
            {
                _trackService.UpdateTrack(id, trackRequest);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "not found")
                    return NotFound();
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
