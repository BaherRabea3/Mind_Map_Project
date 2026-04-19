using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    public class TrackController : CustomControllerBase
    {
        private readonly ITrackService _trackService;
        private readonly IEnrollmentService _enrollmentService;
        public TrackController(ITrackService trackService , IEnrollmentService enrollmentService)
        {
            _trackService = trackService;
            _enrollmentService = enrollmentService;
        }



        /// <summary>
        /// get all tracks with pagination
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="pageSize"> page size</param>
        /// <param name="searchTirm"> search tirm </param>
        /// <returns>list of tracks</returns>
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

        /// <summary>
        /// get tracks with most enrollments
        /// </summary>
        /// <param name="amount">number of tracks you want</param>
        /// <returns>list of tracks</returns>
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

        /// <summary>
        /// get all roadmaps in a track
        /// </summary>
        /// <param name="id">track id</param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">page size</param>
        /// <returns>list of all roadmaps in a track</returns>
        [HttpGet("{id:int}")]
        public ActionResult GetTrackRoadmaps(
            [FromRoute] int id ,
            [FromQuery] int page = 1 ,
            [FromQuery] int pageSize = 6)
        {
            try
            {
                var response = _trackService.GetRoadmapsByTrackId(id, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound("track not found");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// member enroll to the track
        /// </summary>
        /// <param name="trackId"> track id</param>
        /// <returns>enroll info</returns>
        [HttpPost("{trackId:int}/enroll")]
        [Authorize(Roles = "Member")]
        public ActionResult Enroll([FromRoute] int trackId)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            try
            {
                var enrollmentResoponse = _enrollmentService.Enroll(trackId, userId);
                return CreatedAtAction(nameof(Enroll)
                    , "track"
                    , new { trackId = enrollmentResoponse.trackId }
                    , enrollmentResoponse);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound(ex.Message);

                return Conflict(ex.Message);
            }
        }

        /// <summary>
        /// add new track
        /// </summary>
        /// <param name="TrackRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// delete a track
        /// </summary>
        /// <param name="id">track id</param>
        /// <returns></returns>
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

        /// <summary>
        /// update track
        /// </summary>
        /// <param name="id">track id</param>
        /// <param name="trackRequest">updated track</param>
        /// <returns></returns>
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
