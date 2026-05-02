using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.WebAPI.Controllers
{
    public class ResourceController : CustomControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet("{id:int}")]
        public ActionResult GetByid([FromRoute] int id)
        {
            var resourceResponse = _resourceService.GetWithDetails(id);
            return Ok(resourceResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ResourceRequest resourceDto)
        {
            int resourceId = _resourceService.AddResource(resourceDto);
            return CreatedAtAction(nameof(GetByid), "Topic", new { id = resourceId }, new { id = resourceId });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, ResourceRequest resourceDto)
        {
            _resourceService.UpdateResource(id, resourceDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _resourceService.DeleteResource(id);
            return NoContent();
        }

    }
}
