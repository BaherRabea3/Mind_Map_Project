
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ISearchRepository
    {
        public SearchResponse Search(string keyword, int page);
    }
}