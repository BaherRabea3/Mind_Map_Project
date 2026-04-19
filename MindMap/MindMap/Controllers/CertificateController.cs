
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    public class CertificatesController : CustomControllerBase
    {
        private readonly ICertificateService _certService;

        public CertificatesController(ICertificateService certService)
        {
            _certService = certService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        
        [HttpGet]
        [Authorize]
        public ActionResult GetMyCertificates()
        {
            var certs = _certService.GetMyCertificates(GetUserId());
            return Ok(certs);
        }

      
        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult GetById(int id)
        {
            try
            {
                var cert = _certService.GetById(id, GetUserId());
                return Ok(cert);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                if (ex.Message.Contains("forbidden"))
                    return Forbid();
                return BadRequest(ex.Message);
            }
        }
    }
}