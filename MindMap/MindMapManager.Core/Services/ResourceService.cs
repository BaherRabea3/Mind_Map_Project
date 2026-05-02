using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
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
                throw new NotFoundException("topic not found");
            }
            var resourceName = resourceRequest.resourceName.Trim();
            if (string.IsNullOrWhiteSpace(resourceName))
                throw new BadRequestException("resouce name is required");

            var resourceUrl = resourceRequest.rsourceUrl.Trim();
            if (string.IsNullOrWhiteSpace(resourceUrl))
                throw new BadRequestException("resouce url is required");

            bool exists = _resourceRepo
                .IsExist(resourceName, resourceUrl,resourceRequest.topicId, null);

            if (exists)
                throw new ConflictException("resource already existed");

            Resource resource = new Resource();
            resource.Name = resourceName;
            resource.Order = resourceRequest.resourceOrder;
            resource.Paid = resourceRequest.IsPaid;
            resource.ResUrl = resourceUrl;
            resource.TopicId = resourceRequest.topicId;

            _resourceRepo.Add(resource);

            _resourceRepo.Save();
            return resource.ResId;
        }

        public void DeleteResource(int id)
        {
            var resource = _resourceRepo.GetById(id);
            if (resource == null)
                throw new NotFoundException("resource not found");

            _resourceRepo.Delete(resource);

            _resourceRepo.Save();
        }

        public ResourceResponse GetWithDetails(int id)
        {
            var resource = _resourceRepo.GetById(id);

            if (resource == null)
                throw new NotFoundException("resource not found");

            return new ResourceResponse()
                {
                    resourceId = resource.ResId,
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
            if (resource == null) throw new NotFoundException("resource not found");

            var newName = string.IsNullOrWhiteSpace(resourceRequest.resourceName)
                ? resource.Name : resourceRequest.resourceName.Trim();

            var newUrl = resourceRequest.rsourceUrl.Trim();
            if (string.IsNullOrWhiteSpace(newUrl))
                throw new BadRequestException("resource url is required");


            if (resource.Name == newName &&
                   resource.ResUrl == newUrl &&
                   resource.TopicId == resourceRequest.topicId &&
                   resource.Order == resourceRequest.resourceOrder &&
                   resource.Type == resourceRequest.resourceType &&
                   resource.Paid == resourceRequest.IsPaid)
            {
                return;
            }


            bool exists = _resourceRepo.IsExist(newName, newUrl,resourceRequest.topicId, id);
            if (exists) throw new ConflictException("resource already existed");


            resource.Name = newName;
            resource.Order = resourceRequest.resourceOrder;
            resource.Type = resourceRequest.resourceType;
            resource.Paid = resourceRequest.IsPaid;
            resource.ResUrl = newUrl;


            _resourceRepo.Update(resource);
            _resourceRepo.Save();
        }
    }
}
