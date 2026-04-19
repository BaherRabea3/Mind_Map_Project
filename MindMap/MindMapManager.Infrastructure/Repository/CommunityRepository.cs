
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace MindMapManager.Infrastructure.Repository
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly AppDbContext _context;

        public CommunityRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetTopicComments(int topicId)
        {
            return _context.Comments
                .Include(c => c.InverseParentCom)
                .Include(c => c.UserComment)
                .Where(c => c.TopicId == topicId && c.ParentComId == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

        public Comment? GetById(int comId)
        {
            return _context.Comments
                .Include(c => c.UserComment)
                .FirstOrDefault(c => c.ComId == comId);
        }

        public void Add(Comment comment, int userId)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();

            var userComment = new UserComment
            {
                ComId = comment.ComId,
                UserId = userId
            };
            _context.UserComments.Add(userComment);
        }

        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool IsUserEnrolledInTopic(int userId, int topicId)
        {
            var topic = _context.Topics
                .Include(t => t.LidNavigation)
                    .ThenInclude(l => l!.RidNavigation)
                        .ThenInclude(r => r!.Track)
                .FirstOrDefault(t => t.TopicId == topicId);

            if (topic?.LidNavigation?.RidNavigation?.Track == null)
                return false;

            int trackId = topic.LidNavigation.RidNavigation.Track.TrackId;

            return _context.Users
                .Include(u => u.Tracks)
                .Any(u => u.Id == userId && u.Tracks.Any(t => t.TrackId == trackId));
        }
    }
}