using MindMapManager.Core.DTOs;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IResourceService
    {
        public ResourceResponse GetWithDetails(int id);
        public int AddResource(ResourceRequest resourceRequest);
        public void UpdateResource(int id, ResourceRequest resourceRequest);
        public void DeleteResource(int id);
    }
}
