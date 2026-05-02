
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IProgressRepository _progressRepo;
        private readonly IRoadmapRepository _roadmapRepo;

        public ReviewService(IReviewRepository reviewRepo, IProgressRepository progressRepo, IRoadmapRepository roadmapRepo)
        {
            _reviewRepo = reviewRepo;
            _progressRepo = progressRepo;
            _roadmapRepo = roadmapRepo;
        }

        public PagedResult<ReviewResponse> GetRoadmapReviews(int roadmapId, int page, int pageSize)
        {
            var query = _reviewRepo.GetByRoadmapId(roadmapId);

            return new PagedResult<ReviewResponse>
            {
                Items = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(MapToResponse)
                    .ToList(),
                TotalCount = query.Count(),
                Page = page,
                PageSize = pageSize
            };
        }

        public void AddReview(int userId, int roadmapId, ReviewRequest request)
        {
            if (request.Rate < 1 || request.Rate > 5)
                throw new BadRequestException("rate must be between 1 and 5");


            var roadmap = _roadmapRepo.GetById(roadmapId);
            if (roadmap == null)
                throw new NotFoundException("Roadmap not found");


            bool isCompleted = _progressRepo.IsRoadmapCompleted(userId, roadmapId);

            if (!isCompleted)
                throw new ForbiddenException("you must complete the roadmap before reviewing");

            var exists = _reviewRepo.GetByUserAndRoadmap(userId, roadmapId);
            if (exists != null)
                throw new ConflictException("already reviewed");


            var content = request.Content?.Trim();
            if (string.IsNullOrWhiteSpace(content))
                throw new BadRequestException("Review content is required");


            var review = new Review
            {
                UserId = userId,
                Rid = roadmapId,
                Content = request.Content,
                Rate = request.Rate,
                CreatedAt = DateTime.UtcNow
            };

            _reviewRepo.Add(review);
            _reviewRepo.Save();
        }

        public void DeleteReview(int reviewId, int userId, bool isAdmin)
        {
            var review = _reviewRepo.GetById(reviewId);
            if (review == null)
                throw new NotFoundException("review not found");

            if (!isAdmin && review.UserId != userId)
                throw new ForbiddenException("You are not allowed to delete this review");

            _reviewRepo.Delete(review);
            _reviewRepo.Save();
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