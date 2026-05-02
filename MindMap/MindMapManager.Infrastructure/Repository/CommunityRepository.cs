
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
                  .Where(c => c.TopicId == topicId && c.ParentComId == null)
                  .Include(c => c.UserComment)                 
                  .Include(c => c.InverseParentCom)            
                      .ThenInclude(r => r.UserComment)         
                  .OrderByDescending(c => c.CreatedAt)
                  .ToList();

        }

        public Comment? GetById(int comId)
        {
            return _context.Comments
                .Include(c => c.UserComment)
                .Include(c => c.InverseParentCom)
                    .ThenInclude(c => c.UserComment)
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
        public void DeleteUserComment(UserComment userComment)
        {
            _context.UserComments.Remove(userComment);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

       
    }
}