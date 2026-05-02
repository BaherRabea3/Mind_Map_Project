
using MindMapManager.Core.Entities;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ICommunityRepository
    {
        public List<Comment> GetTopicComments(int topicId);
        public Comment? GetById(int comId);
        public void Add(Comment comment, int userId);
        public void Delete(Comment comment);
        void DeleteUserComment(UserComment userComment);
        public void Save();
    }
}