
using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ISearchService
    {
        public SearchResponse Search(string keyword, int page);
    }
}
