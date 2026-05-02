using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _levelRepo;
        private readonly IRoadmapRepository _roadmapRepo;

        public LevelService(ILevelRepository levelRepo, IRoadmapRepository roadmapRepo)
        {
            _levelRepo = levelRepo;
            _roadmapRepo = roadmapRepo;
        }

        public int AddLevel(LevelRequest levelRequest)
        {
            var roadmap = _roadmapRepo.GetById(levelRequest.roadmapId);
            if (roadmap == null)
                throw new NotFoundException("roadmap not found");


            var levelName = levelRequest.name?.Trim();

            if (string.IsNullOrWhiteSpace(levelName))
                throw new BadRequestException("Level name is required");


            bool isExisted = _levelRepo.IsExist(levelRequest.name, levelRequest.roadmapId, 0);

            if (isExisted)
                throw new ConflictException("level already existed");

            Level level = new Level();
            level.Name = levelName;
            level.Rid = levelRequest.roadmapId;

            _levelRepo.Add(level);

            _levelRepo.Save();
            return level.Lid;
        }

        public void DeleteLevel(int id)
        {
            var level = _levelRepo.GetById(id);
            if (level == null)
                throw new NotFoundException("level not found");

            _levelRepo.Delete(level);

            _levelRepo.Save();
        }

        public LevelResoponse GetWithDetails(int id)
        {
            var level = _levelRepo.GetByidWithDetails(id);

            if (level == null)
                throw new NotFoundException("level not found");

            return new LevelResoponse()
            {
                levelId = level.Lid,
                levelName= level.Name,
                topicResponses = level.Topics
                .OrderBy(t => t.Order)
                .Select(topic => new TopicResponse()
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
                })     
            };
        }

        public void UpdateLevel(int id, LevelRequest levelRequest)
        {
            var level = _levelRepo.GetById(id);
            if (level == null) throw new NotFoundException("level not found");

            

            var newLevelName = string.IsNullOrWhiteSpace(levelRequest.name) ? level.Name : levelRequest.name.Trim();
           
            var newRoadmapId = levelRequest.roadmapId;

            if (level.Name == newLevelName && level.Rid == newRoadmapId)
                return;

            bool isExisted = _levelRepo.IsExist(newLevelName,newRoadmapId,id);

            if (isExisted) throw new ConflictException("level already existed");

            level.Name = newLevelName;
            level.Rid = newRoadmapId;

            _levelRepo.Update(level);
            _levelRepo.Save();
               
        }
    }
}
