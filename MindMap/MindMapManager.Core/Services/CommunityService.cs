
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class CommunityService : ICommunityService
    {
        private readonly ICommunityRepository _commentRepo;
        private readonly IUserTrackRepository _userTrackRepo;
        private readonly ITopicRepository _topicRepo;
        public CommunityService(ICommunityRepository commentRepo, IUserTrackRepository userTrackRepo, ITopicRepository topicRepo)
        {
            _commentRepo = commentRepo;
            _userTrackRepo = userTrackRepo;
            _topicRepo = topicRepo;
        }

        public List<CommunityResponse> GetTopicComments(int topicId, int userId)
        {
            var topic = _topicRepo.GetByIdWithLevelAndRoadmaps(topicId);
            if (topic == null)
                throw new NotFoundException("topic not found");

            var trackId = topic.LidNavigation!.RidNavigation!.TrackId;
            if (!_userTrackRepo.IsEnrolled(userId, trackId))
                throw new ForbiddenException(" Enroll into track to access the community.");

            var comments = _commentRepo.GetTopicComments(topicId);
            return comments.Select(MapToResponse).ToList();
        }

        public void AddComment(int topicId, int userId, CommunityRequest request)
        {
            var topic = _topicRepo.GetByIdWithLevelAndRoadmaps(topicId);
            if (topic == null)
                throw new NotFoundException("topic not found");

            var trackId = topic.LidNavigation!.RidNavigation!.TrackId;
            if (!_userTrackRepo.IsEnrolled(userId, trackId))
                throw new ForbiddenException("Enroll into track to access the community.");

            var comment = new Comment
            {
                Content = request.Content,
                TopicId = topicId,
                ParentComId = null,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepo.Add(comment, userId);

            _commentRepo.Save();
        }

        public void ReplyToComment(int commentId, int userId, CommunityRequest request)
        {
            var parent = _commentRepo.GetById(commentId);
            if (parent == null)
                throw new NotFoundException("comment not found");

            var topic = _topicRepo.GetByIdWithLevelAndRoadmaps(parent.TopicId!.Value);
            if (topic == null)
                throw new NotFoundException("topic not found");

            var trackId = topic.LidNavigation!.RidNavigation!.TrackId;
            if (!_userTrackRepo.IsEnrolled(userId, trackId))
                throw new ForbiddenException("Enroll into track to access the community.");

            var reply = new Comment
            {
                Content = request.Content,
                TopicId = parent.TopicId,
                ParentComId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            _commentRepo.Add(reply, userId);

            _commentRepo.Save();
        }

        public void DeleteComment(int commentId, int userId, bool isAdmin)
        {
            var comment = _commentRepo.GetById(commentId);
            if (comment == null)
                throw new NotFoundException("not found");

            if (!isAdmin && comment.UserComment?.UserId != userId)
                throw new ForbiddenException("You are not allowed to delete this comment");


            DeleteCommentTree(comment);

            _commentRepo.Save();
        }

        private void DeleteCommentTree(Comment comment)
        {
            if (comment.InverseParentCom != null && comment.InverseParentCom.Any())
            {
                foreach (var reply in comment.InverseParentCom.ToList())
                {
                    DeleteCommentTree(reply);
                }
            }

            if (comment.UserComment != null)
                _commentRepo.DeleteUserComment(comment.UserComment);

            _commentRepo.Delete(comment);
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