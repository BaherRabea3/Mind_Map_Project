
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IProgressRepository _progressRepo;

        public ReviewService(IReviewRepository reviewRepo, IProgressRepository progressRepo)
        {
            _reviewRepo = reviewRepo;
            _progressRepo = progressRepo;
        }

        public List<ReviewResponse> GetRoadmapReviews(int roadmapId)
        {
            var reviews = _reviewRepo.GetByRoadmapId(roadmapId);
            return reviews.Select(MapToResponse).ToList();
        }

        public void AddReview(int userId, int roadmapId, ReviewRequest request)
        {
            if (request.Rate < 1 || request.Rate > 5)
                throw new Exception("rate must be between 1 and 5");

            bool isCompleted = _progressRepo.IsRoadmapCompleted(userId, roadmapId);
            if (!isCompleted)
                throw new Exception("you must complete the roadmap before reviewing");

            var existing = _reviewRepo.GetByUserAndRoadmap(userId, roadmapId);
            if (existing != null)
                throw new Exception("already reviewed");

            var review = new Review
            {
                UserId = userId,
                Rid = roadmapId,
                Content = request.Content,
                Rate = request.Rate,
                CreatedAt = DateTime.UtcNow
            };

            _reviewRepo.Add(review);
            try
            {
                _reviewRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        public void DeleteReview(int reviewId, int userId, bool isAdmin)
        {
            var review = _reviewRepo.GetById(reviewId);
            if (review == null)
                throw new Exception("not found");

            if (!isAdmin && review.UserId != userId)
                throw new Exception("forbidden");

            _reviewRepo.Delete(review);
            try
            {
                _reviewRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        private static ReviewResponse MapToResponse(Review review) => new()
        {
            RevId = review.RevId,
            Content = review.Content ?? string.Empty,
            Rate = review.Rate,
            CreatedAt = review.CreatedAt,
            UserId = review.UserId ?? 0,
            RoadmapId = review.Rid ?? 0
        };
    }
}