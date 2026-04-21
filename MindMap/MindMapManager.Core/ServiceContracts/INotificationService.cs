
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Helpers;

namespace MindMapManager.Core.ServiceContracts
{
    public interface INotificationService
    {
        public PagedResult<NotificationResponse> GetMyNotifications(int userId, int page, int pageSize);
        public int GetUnreadCount(int userId);
        public void MarkAsRead(int notId, int userId);
        public void MarkAllAsRead(int userId);
        public void CreateNotification(int userId, string message, string refType, int refId);
    }
}