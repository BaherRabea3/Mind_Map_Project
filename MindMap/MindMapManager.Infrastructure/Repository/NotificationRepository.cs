
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Notification> GetUserNotifications(int userId)
        {
            return _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderBy(n => n.Read)
                .ThenByDescending(n => n.CreatedAt)
                .ToList();
        }

        public int GetUnreadCount(int userId)
        {
            return _context.Notifications
                .Count(n => n.UserId == userId && !n.Read);
        }

        public Notification? GetById(int id)
        {
            return _context.Notifications.Find(id);
        }

        public void MarkAsRead(Notification notification)
        {
            notification.Read = true;
            _context.Update(notification);
        }

        public void MarkAllAsRead(int userId)
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == userId && !n.Read)
                .ToList();

            foreach (var n in notifications)
                n.Read = true;
        }

        public void Add(Notification notification)
        {
            _context.Add(notification);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}