using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;
using MindMapManager.Infrastructure.Repository;

namespace MindMapManager.WebAPI.Controllers
{
   
    public class TopicController : CustomControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet("{id:int}")]
        public ActionResult GetByid([FromRoute]int id)
        {
            try
            {
                var topicResponse = _topicService.GetWithDetails(id);
                return Ok(topicResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(TopicRequest topicDto)
        {
            try
            {
                int topicId = _topicService.AddTopic(topicDto);
                return CreatedAtAction(nameof(GetByid), "Topic", new { id = topicId }, new { id = topicId });
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
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id ,TopicRequest topicDto)
        {
            try
            {
                _topicService.UpdateTopic(id, topicDto);
                return NoContent();
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
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                _topicService.DeleteTopic(id);
                return NoContent();
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

    }
}
