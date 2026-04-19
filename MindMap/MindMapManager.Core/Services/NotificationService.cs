
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notifRepo;

        public NotificationService(INotificationRepository notifRepo)
        {
            _notifRepo = notifRepo;
        }

        public List<NotificationResponse> GetMyNotifications(int userId)
        {
            return _notifRepo.GetUserNotifications(userId)
                .Select(MapToResponse).ToList();
        }

        public int GetUnreadCount(int userId)
        {
            return _notifRepo.GetUnreadCount(userId);
        }

        public void MarkAsRead(int notId, int userId)
        {
            var notif = _notifRepo.GetById(notId);
            if (notif == null || notif.UserId != userId)
                throw new Exception("not found");

            _notifRepo.MarkAsRead(notif);
            _notifRepo.Save();
        }

        public void MarkAllAsRead(int userId)
        {
            _notifRepo.MarkAllAsRead(userId);
            _notifRepo.Save();
        }

        public void CreateNotification(int userId, string message, string refType, int refId)
        {
            var notif = new Notification
            {
                UserId = userId,
                Message = message,
                RefType = refType,
                RefId = refId,
                Read = false,
                CreatedAt = DateTime.UtcNow
            };

            _notifRepo.Add(notif);
            _notifRepo.Save();
        }

        private static NotificationResponse MapToResponse(Notification n) => new()
        {
            NotId = n.NotId,
            Message = n.Message,
            Read = n.Read,
            CreatedAt = n.CreatedAt,
            RefType = n.RefType,
            RefId = n.RefId
        };
    }
}
