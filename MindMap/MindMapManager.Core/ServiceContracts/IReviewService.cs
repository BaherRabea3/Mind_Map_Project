
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Helpers;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IReviewService
    {
        public PagedResult<ReviewResponse> GetRoadmapReviews(int roadmapId, int page, int pageSize);
        public void AddReview(int userId, int roadmapId, ReviewRequest request);
        public void DeleteReview(int reviewId, int userId, bool isAdmin);
    }
}