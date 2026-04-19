
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ICommunityService
    {
        public List<CommunityResponse> GetTopicComments(int topicId, int userId);
        public void AddComment(int topicId, int userId, CommunityRequest request);
        public void ReplyToComment(int commentId, int userId, CommunityRequest request);
        public void DeleteComment(int commentId, int userId, bool isAdmin);
    }
}
