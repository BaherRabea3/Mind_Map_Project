
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IReviewService
    {
        public List<ReviewResponse> GetRoadmapReviews(int roadmapId);
        public void AddReview(int userId, int roadmapId, ReviewRequest request);
        public void DeleteReview(int reviewId, int userId, bool isAdmin);
    }
}