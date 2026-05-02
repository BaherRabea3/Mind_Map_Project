
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.Helpers;
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

        public PagedResult<NotificationResponse> GetMyNotifications(int userId, int page, int pageSize)
        {
            var query = _notifRepo.GetUserNotifications(userId);

            return new PagedResult<NotificationResponse>
            {
                Items = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(MapToResponse)
                    .ToList(),
                TotalCount = query.Count(),
                Page = page,
                PageSize = pageSize
            };
        }

        public int GetUnreadCount(int userId)
        {
            return _notifRepo.GetUnreadCount(userId);
        }

        public void MarkAsRead(int notId, int userId)
        {
            var notif = _notifRepo.GetById(notId);
            if (notif == null || notif.UserId != userId)
                throw new NotFoundException("not found");

            _notifRepo.MarkAsRead(notif);
            _notifRepo.Save();
        }

        public void MarkAllAsRead(int userId)
        {
            _notifRepo.MarkAllAsRead(userId);
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
