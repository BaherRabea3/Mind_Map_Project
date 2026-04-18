
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize]
    public class BookmarksController : CustomControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        
        [HttpGet]
        public ActionResult GetMyBookmarks()
        {
            var bookmarks = _bookmarkService.GetMyBookmarks(GetUserId());
            return Ok(bookmarks);
        }

        
        [HttpPost("{resourceId:int}")]
        public ActionResult AddBookmark(int resourceId)
        {
            try
            {
                _bookmarkService.AddBookmark(GetUserId(), resourceId);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       
        [HttpDelete("{resourceId:int}")]
        public ActionResult RemoveBookmark(int resourceId)
        {
            try
            {
                _bookmarkService.RemoveBookmark(GetUserId(), resourceId);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }
    }
}