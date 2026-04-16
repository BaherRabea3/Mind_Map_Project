using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.ServiceContracts;
using MindMapManager.Core.Services;

namespace MindMapManager.WebAPI.Controllers
{
    public class LevelController : CustomControllerBase
    {
        private readonly ILevelService _levelService;

        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
        }

        [HttpGet("{id:int}")]
        public ActionResult GetByid([FromRoute] int id)
        {
            try
            {
                var levelResponse = _levelService.GetWithDetails(id);
                return Ok(levelResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(LevelRequest levelDto)
        {
            try
            {
                int levelId = _levelService.AddLevel(levelDto);
                return CreatedAtAction(nameof(GetByid), "Topic", new { id =  levelId}, new { id = levelId });
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
        public ActionResult Update(int id, LevelRequest levelDto)
        {
            try
            {
                _levelService.UpdateLevel(id, levelDto);
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
                _levelService.DeleteLevel(id);
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
