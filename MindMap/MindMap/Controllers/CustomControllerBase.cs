using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MindMapManager.WebAPI.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
    }
}
