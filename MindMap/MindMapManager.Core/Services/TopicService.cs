using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
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
                throw new Exception("level not found");

            Topic topic = new Topic();
            topic.Name = topicRequest.name;
            topic.Order = topicRequest.Order;
            topic.Lid = topicRequest.levelId;

            bool isExisted = _topicRepo.IsExist(topic);

            if (isExisted)
                throw new Exception("topic already existed");

            _topicRepo.Add(topic);

            try
            {
                _topicRepo.Save();
                return topic.TopicId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
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
                throw new Exception("not found");

            return new TopicResponse()
            {
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
            if (topic == null) throw new Exception("not found");

            if (topic.Name == topicRequest.name && topic.Lid == topicRequest.levelId)
                return;

            topic.Name = topicRequest.name;
            topic.Order = topicRequest.Order;
            topic.Lid = topicRequest.levelId;

            bool isExisted = _topicRepo.IsExist(topic);
            if (isExisted) throw new Exception("already existed");

            _topicRepo.Update(topic);
            try
            {
                _topicRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }
    }
}
