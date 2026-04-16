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
            try
            {
                var resourceResponse = _resourceService.GetWithDetails(id);
                return Ok(resourceResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ResourceRequest resourceDto)
        {
            try
            {
                int resourceId = _resourceService.AddResource(resourceDto);
                return CreatedAtAction(nameof(GetByid), "Topic", new { id = resourceId }, new { id = resourceId });
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
        public ActionResult Update(int id, ResourceRequest resourceDto)
        {
            try
            {
                _resourceService.UpdateResource(id, resourceDto);
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
                _resourceService.DeleteResource(id);
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
