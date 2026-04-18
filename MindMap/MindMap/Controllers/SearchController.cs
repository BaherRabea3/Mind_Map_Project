
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    public class SearchController : CustomControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

       
        [HttpGet("/api/search")]
        public ActionResult Search([FromQuery] string q, [FromQuery] int page = 1)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("keyword is required");

            var result = _searchService.Search(q, page);
            return Ok(result);
        }
    }
}