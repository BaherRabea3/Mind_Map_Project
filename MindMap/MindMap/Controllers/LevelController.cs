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
            var levelResponse = _levelService.GetWithDetails(id);
            return Ok(levelResponse);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(LevelRequest levelDto)
        {
            int levelId = _levelService.AddLevel(levelDto);
            return CreatedAtAction(nameof(GetByid), "Topic", new { id = levelId }, new { id = levelId });
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, LevelRequest levelDto)
        {
            _levelService.UpdateLevel(id, levelDto);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _levelService.DeleteLevel(id);
            return NoContent();
        }
    }
}
