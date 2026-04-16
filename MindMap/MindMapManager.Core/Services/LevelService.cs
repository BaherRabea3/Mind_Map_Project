using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
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

        public int AddLevel(LevelRequest LevelRequest)
        {
            var roadmap = _roadmapRepo.GetById(LevelRequest.roadmapId);
            if (roadmap == null)
                throw new Exception("roadmap not found");

            Level level = new Level();
            level.Name = LevelRequest.name;
           
           

            bool isExisted = _levelRepo.IsExist(level);

            if (isExisted)
                throw new Exception("level already existed");

            _levelRepo.Add(level);

            try
            {
                _levelRepo.Save();
                return level.Lid;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }

        public void DeleteLevel(int id)
        {
            var roadmap = _levelRepo.GetById(id);
            if (roadmap == null)
                throw new Exception("not found");

            _levelRepo.Delete(roadmap);

            try
            {
                _levelRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed : {ex.Message}");
            }
        }

        public LevelResoponse GetWithDetails(int id)
        {
            var level = _levelRepo.GetByidWithDetails(id);

            if (level == null)
                throw new Exception("not found");

            return new LevelResoponse()
            {
                levelName= level.Name,
                topicResponses = level.Topics
                .OrderBy(t => t.Order)
                .Select(topic => new TopicResponse()
                {
                    topicName = topic.Name,
                    topicOrder = topic.Order,
                    resources = topic.Resources
                .OrderBy(r => r.Order)
                .Select(r => new ResourceResponse()
                {
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
            if (level == null) throw new Exception("not found");

            if (level.Name == levelRequest.name && level.Rid == levelRequest.roadmapId)
                return;

            level.Name = levelRequest.name;
            level.Rid = levelRequest.roadmapId;

            bool isExisted = _levelRepo.IsExist(level);
            if (isExisted) throw new Exception("already existed");

            _levelRepo.Update(level);
            try
            {
                _levelRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }
    }
}
