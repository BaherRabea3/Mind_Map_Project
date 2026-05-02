
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface INotificationRepository
    {
        public IQueryable<Notification> GetUserNotifications(int userId);
        public int GetUnreadCount(int userId);
        public Notification? GetById(int id);
        public void MarkAsRead(Notification notification);
        public void MarkAllAsRead(int userId);
        public void Add(Notification notification);
        public void Save();
    }
}