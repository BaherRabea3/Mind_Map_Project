
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _commentRepo;

        public CommunityService(ICommunityRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public List<CommunityResponse> GetTopicComments(int topicId, int userId)
        {
            if (!_commentRepo.IsUserEnrolledInTopic(userId, topicId))
                throw new Exception("forbidden: Join the track to access the community.");

            var comments = _commentRepo.GetTopicComments(topicId);
            return comments.Select(MapToResponse).ToList();
        }

        public void AddComment(int topicId, int userId, CommunityRequest request)
        {
            if (!_commentRepo.IsUserEnrolledInTopic(userId, topicId))
                throw new Exception("forbidden: Join the track to access the community.");

            var comment = new Comment
            {
                Content = request.Content,
                TopicId = topicId,
                ParentComId = null,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepo.Add(comment, userId);

            try
            {
                _commentRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        public void ReplyToComment(int commentId, int userId, CommunityRequest request)
        {
            var parent = _commentRepo.GetById(commentId);
            if (parent == null)
                throw new Exception("not found");

            if (!_commentRepo.IsUserEnrolledInTopic(userId, parent.TopicId ?? 0))
                throw new Exception("forbidden: Join the track to access the community.");

            var reply = new Comment
            {
                Content = request.Content,
                TopicId = parent.TopicId,
                ParentComId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepo.Add(reply, userId);

            try
            {
                _commentRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        public void DeleteComment(int commentId, int userId, bool isAdmin)
        {
            var comment = _commentRepo.GetById(commentId);
            if (comment == null)
                throw new Exception("not found");

            if (!isAdmin && comment.UserComment?.UserId != userId)
                throw new Exception("forbidden");

            _commentRepo.Delete(comment);

            try
            {
                _commentRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        private static CommunityResponse MapToResponse(Comment c) => new()
        {
            ComId = c.ComId,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            UserId = c.UserComment?.UserId,
            Replies = c.InverseParentCom.Select(MapToResponse).ToList()
        };
    }
}