

using MindMapManager.Core.DTOs;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepo;

        public SearchService(ISearchRepository searchRepo)
        {
            _searchRepo = searchRepo;
        }

        public SearchResponse Search(string keyword, int page)
        {
            return _searchRepo.Search(keyword, page);
        }
    }
}