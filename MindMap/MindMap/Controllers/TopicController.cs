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
        public ActionResult GetByid([FromRoute] int id)
        {
            var topicResponse = _topicService.GetWithDetails(id);
            return Ok(topicResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(TopicRequest topicDto)
        {
            int topicId = _topicService.AddTopic(topicDto);
            return CreatedAtAction(nameof(GetByid), "Topic", new { id = topicId }, new { id = topicId });
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, TopicRequest topicDto)
        {
            _topicService.UpdateTopic(id, topicDto);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _topicService.DeleteTopic(id);
            return NoContent();
        }
    }
}
