
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
        public ActionResult GetMyBookmarks( [FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var bookmarks = _bookmarkService.GetMyBookmarks(GetUserId(), page, pageSize);
            return Ok(bookmarks);
        }


        [HttpPost("{resourceId:int}")]
        public ActionResult AddBookmark(int resourceId)
        {
            _bookmarkService.AddBookmark(GetUserId(), resourceId);
             return StatusCode(StatusCodes.Status201Created);
        }

       
        [HttpDelete("{resourceId:int}")]
        public ActionResult RemoveBookmark(int resourceId)
        {
            _bookmarkService.RemoveBookmark(GetUserId(), resourceId);
            return NoContent();
        }
    }
}