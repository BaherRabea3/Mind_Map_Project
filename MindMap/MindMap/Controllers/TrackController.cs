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
            var Response = _trackService.GetAll(page, pageSize, searchTirm);
            return Ok(Response);
        }

        /// <summary>
        /// get tracks with most enrollments
        /// </summary>
        /// <param name="amount">number of tracks you want</param>
        /// <returns>list of tracks</returns>
        [HttpGet("Featured-tracks")]
        public IActionResult FeaturedTracks([FromQuery] int amount = 4)
        {
            var Response = _trackService.GetTracksWithMostEnrollments(amount);
            return Ok(Response);
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
            [FromRoute] int id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 6)
        {
            var response = _trackService.GetRoadmapsByTrackId(id, page, pageSize);
            return Ok(response);
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
            
                var enrollmentResoponse = _enrollmentService.Enroll(trackId, userId);
            return CreatedAtAction(nameof(Enroll)
                , "track"
                , new { trackId = enrollmentResoponse.trackId }
                , enrollmentResoponse);
            
        }

        [HttpGet("{Id:int}/enrollment-status")]
        public ActionResult EnrollmentStatus(int id)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            bool IsEnrolled = _enrollmentService.IsEnrolled(id, userId);

            return Ok(new {IsEnrolled});
        }
        /// <summary>
        /// add new track
        /// </summary>
        /// <param name="TrackRequest"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Add([FromForm]TrackRequestDto TrackRequest)
        {
             await _trackService.AddTrack(TrackRequest);
             return Created();
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
            _trackService.DeleteTrack(id);
            return NoContent();
        }

        /// <summary>
        /// update track
        /// </summary>
        /// <param name="id">track id</param>
        /// <param name="updateTrackDto">updated track</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Update(int id , [FromForm] UpdateTrackRequestDto updateTrackDto)
        {
           await _trackService.UpdateTrack(id, updateTrackDto);
           return NoContent();
        }
    }
}
