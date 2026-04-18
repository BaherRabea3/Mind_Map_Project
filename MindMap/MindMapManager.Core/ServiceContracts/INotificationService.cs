
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface INotificationService
    {
        public List<NotificationResponse> GetMyNotifications(int userId);
        public int GetUnreadCount(int userId);
        public void MarkAsRead(int notId, int userId);
        public void MarkAllAsRead(int userId);
        public void CreateNotification(int userId, string message, string refType, int refId);
    }
}