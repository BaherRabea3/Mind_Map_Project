using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepo;
        private readonly ITopicRepository _topicRepo;

        public ResourceService(IResourceRepository ResourceRepo , ITopicRepository topicRepo)
        {
             _resourceRepo = ResourceRepo;
            _topicRepo = topicRepo;
        }

        public int AddResource(ResourceRequest resourceRequest)
        {
            var topic = _topicRepo.GetById(resourceRequest.topicId);

            if (topic == null)
            {
                throw new Exception("topic not found");
            }

            Resource resource = new Resource();
            resource.Name = resourceRequest.resourceName;
            resource.Order = resourceRequest.resourceOrder;
            resource.Paid = resourceRequest.IsPaid;
            resource.ResUrl = resourceRequest.rsourceUrl;
            resource.TopicId = resourceRequest.topicId;


            bool isExisted = _resourceRepo.IsExist(resource);

            if (isExisted)
                throw new Exception("resource already existed");

            _resourceRepo.Add(resource);

            try
            {
                _resourceRepo.Save();
                return resource.ResId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        public void DeleteResource(int id)
        {
            var resource = _resourceRepo.GetById(id);
            if (resource == null)
                throw new Exception("not found");

            _resourceRepo.Delete(resource);

            try
            {
                _resourceRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed : {ex.Message}");
            }
        }

        public ResourceResponse GetWithDetails(int id)
        {
            var resource = _resourceRepo.GetById(id);

            if (resource == null)
                throw new Exception("not found");

            return new ResourceResponse()
                {
                    resourceName = resource.Name,
                    resourceOrder = resource.Order,
                    resourceType = resource.Type,
                    rsourceUrl = resource.ResUrl,
                    IsPaid = resource.Paid,
                };
        }

        public void UpdateResource(int id, ResourceRequest resourceRequest)
        {
            var resource = _resourceRepo.GetById(id);
            if (resource == null) throw new Exception("not found");

            if (resource.Name == resourceRequest.resourceName && resource.ResUrl == resourceRequest.rsourceUrl)
                return;

            resource.Name = resourceRequest.resourceName;
            resource.Order = resourceRequest.resourceOrder;
            resource.Type = resourceRequest.resourceType;
            resource.Paid = resourceRequest.IsPaid;
            resource.ResUrl = resourceRequest.rsourceUrl;

            bool isExisted = _resourceRepo.IsExist(resource);
            if (isExisted) throw new Exception("already existed");

            _resourceRepo.Update(resource);
            try
            {
                _resourceRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }
    }
}
