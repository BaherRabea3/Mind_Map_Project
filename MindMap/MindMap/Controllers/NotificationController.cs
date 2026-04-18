
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.ServiceContracts;
using System.Security.Claims;

namespace MindMapManager.WebAPI.Controllers
{
    [Authorize]
    public class NotificationsController : CustomControllerBase
    {
        private readonly INotificationService _notifService;

        public NotificationsController(INotificationService notifService)
        {
            _notifService = notifService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public ActionResult GetMyNotifications()
        {
            var notifs = _notifService.GetMyNotifications(GetUserId());
            return Ok(notifs);
        }

      
        [HttpGet("unread-count")]
        public ActionResult GetUnreadCount()
        {
            var count = _notifService.GetUnreadCount(GetUserId());
            return Ok(new { unreadCount = count });
        }

        
        [HttpPut("{id:int}/read")]
        public ActionResult MarkAsRead(int id)
        {
            try
            {
                _notifService.MarkAsRead(id, GetUserId());
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found"))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

      
        [HttpPut("read-all")]
        public ActionResult MarkAllAsRead()
        {
            _notifService.MarkAllAsRead(GetUserId());
            return NoContent();
        }
    }
}