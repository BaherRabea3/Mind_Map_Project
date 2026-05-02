
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize]
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
        public ActionResult GetMyCertificates()
        {
            var certs = _certService.GetMyCertificates(GetUserId());
            return Ok(certs);
        }

      
        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            var cert = _certService.GetById(id, GetUserId());
            return Ok(cert);
        }

        [HttpGet("{id:int}/download")]
        public IActionResult Download(int id)
        {
            var response = _certService.DownloadCertificate(id , GetUserId());
            return PhysicalFile(response.physicalPath, response.contentType, response.fileName);
        }

        [HttpGet("verify/{code}")]
        public ActionResult Verify(string code)
        {
            var response = _certService.Verify(code);

            if (!response.IsValid)
                return Ok(new { IsValid = false });

            return Ok(response);
        }
    }
}