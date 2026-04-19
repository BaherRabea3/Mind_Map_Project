
using MindMapManager.Core.DTOs;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;

namespace MindMapManager.Infrastructure.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly AppDbContext _context;
        private const int PageSize = 5;

        public SearchRepository(AppDbContext context)
        {
            _context = context;
        }

        public SearchResponse Search(string keyword, int page)
        {
            keyword = keyword.ToLower();
            int skip = (page - 1) * PageSize;

            var tracks = _context.Tracks
                .Where(t => t.Name.ToLower().Contains(keyword)
                    || (t.Description != null && t.Description.ToLower().Contains(keyword)))
                .Skip(skip).Take(PageSize)
                .Select(t => new TrackSearchResult
                {
                    TrackId = t.TrackId,
                    Name = t.Name,
                    Description = t.Description
                }).ToList();

            var roadmaps = _context.Roadmaps
                .Where(r => r.Name.ToLower().Contains(keyword)
                    || (r.Description != null && r.Description.ToLower().Contains(keyword)))
                .Skip(skip).Take(PageSize)
                .Select(r => new RoadmapSearchResult
                {
                    Rid = r.Rid,
                    Name = r.Name,
                    Description = r.Description
                }).ToList();

            var topics = _context.Topics
                .Where(t => t.Name.ToLower().Contains(keyword))
                .Skip(skip).Take(PageSize)
                .Select(t => new TopicSearchResult
                {
                    TopicId = t.TopicId,
                    Name = t.Name
                }).ToList();

            return new SearchResponse
            {
                Tracks = tracks,
                Roadmaps = roadmaps,
                Topics = topics
            };
        }
    }
}
