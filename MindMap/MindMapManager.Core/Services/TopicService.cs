using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepo;
        private readonly ILevelRepository _levelRepo;

        public TopicService(ITopicRepository topicRepo, ILevelRepository levelRepo)
        {
            _topicRepo = topicRepo;
            _levelRepo = levelRepo;
        }

        public int AddTopic(TopicRequest topicRequest)
        {
            var level = _levelRepo.GetById(topicRequest.levelId);
            if (level == null)
                throw new NotFoundException("level not found");

            var topicName = topicRequest.name?.Trim();
            if (string.IsNullOrWhiteSpace(topicName))
                throw new BadRequestException("Topic name is required");

            bool isExisted = _topicRepo.IsExist(topicName, topicRequest.levelId, null);

            if (isExisted)
                throw new ConflictException("topic already existed");

            Topic topic = new Topic();
            topic.Name = topicRequest.name;
            topic.Order = topicRequest.Order;
            topic.Lid = topicRequest.levelId;

           
            _topicRepo.Add(topic);
            _topicRepo.Save();

            return topic.TopicId;
        }

        public void DeleteTopic(int id)
        {
            var roadmap = _topicRepo.GetById(id);
            if (roadmap == null)
                throw new Exception("not found");

           _topicRepo.Delete(roadmap);

            try
            {
                _topicRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed : {ex.Message}");
            }
        }

        public TopicResponse GetWithDetails(int id)
        {
           var topic = _topicRepo.GetByIdWithResources(id);

            if (topic == null)
                throw new NotFoundException("topic not found");

            return new TopicResponse()
            {
                topicId = topic.TopicId,
                topicName = topic.Name,
                topicOrder = topic.Order,
                resources = topic.Resources
                .OrderBy(r => r.Order)
                .Select(r => new ResourceResponse()
                {
                    resourceId = r.ResId,
                    resourceName = r.Name,
                    resourceOrder = r.Order,
                    resourceType = r.Type,
                    rsourceUrl = r.ResUrl,
                    IsPaid = r.Paid,
                })
            };
        }

        public void UpdateTopic(int id, TopicRequest topicRequest)
        {
            var topic = _topicRepo.GetById(id);
            if (topic == null) throw new NotFoundException("topic not found");

            var newName = string.IsNullOrWhiteSpace(topicRequest.name)
                ? topic.Name : topicRequest.name.Trim();

            if (topic.Name == topicRequest.name 
                && topic.Lid == topicRequest.levelId
                && topic.Order == topicRequest.Order)
                return;

            bool isExisted = _topicRepo.IsExist(newName, topicRequest.levelId, null);

            if (isExisted)
                throw new ConflictException("topic already existed");

            topic.Name = topicRequest.name;
            topic.Order = topicRequest.Order;
            topic.Lid = topicRequest.levelId;

            _topicRepo.Update(topic);
            _topicRepo.Save();
        }
    }
}
