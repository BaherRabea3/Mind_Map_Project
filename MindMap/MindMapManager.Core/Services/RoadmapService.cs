
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;


namespace MindMapManager.Core.Services
{
    public class RoadmapService : IRoadmapService
    {
        public readonly IRoadmapRepository _roadmapRepo;
        private readonly ITrackRepository _trackRepo;

        public RoadmapService(IRoadmapRepository roadmapRepo , ITrackRepository trackRepo)
        {
            _roadmapRepo = roadmapRepo;
            _trackRepo = trackRepo;
        }

        public RoadmapCreateResponse AddRoadmap(RoadmapRequestDto roadmapDto)
        {
            var track = _trackRepo.GetById(roadmapDto.trackId);

            if (track == null)
            {
                throw new Exception("invalid track id");
            }

            var roadmaDB = _roadmapRepo.FindByName(roadmapDto.RoadmapName);

            if (roadmaDB != null)
            {
                throw new Exception("roadmap already existed");
            }

            var roadmap = new Roadmap();
            roadmap.Name = roadmapDto.RoadmapName;
            roadmap.TrackId = roadmapDto.trackId;
            roadmap.Description = roadmapDto.RoadmapDescription;

            _roadmapRepo.Add(roadmap);
            try
            {
                _roadmapRepo.Save();
               return new RoadmapCreateResponse()
               {
                   id = roadmap.Rid,
                   description = roadmap.Description,
                   name = roadmap.Name,
                   trackName = track.Name,
               };
            }
            catch (Exception ex)
            {
                throw new Exception("internal" + ex.Message);
            }

        }

        public void DeleteRoadmap(int id)
        {
            var roadmap = _roadmapRepo.GetById(id);
            if (roadmap == null)
                throw new Exception("not found");

            _roadmapRepo.Remove(roadmap);

            try
            {
                _roadmapRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"failed : {ex.Message}");
            }
        }

        public PagedResult<RoadmapResponseDto> GetAll(int pageNo, int pageSize, string? searchTirm)
        {
            if (pageNo < 1 || pageSize < 1)
                throw new Exception("invalid pageNo or page size");

            var query = _roadmapRepo.Search(searchTirm);
            var totalcount = query.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalcount / pageSize);

            if(pageNo > totalPages)
                throw new Exception("invalid pageNo");
            if (pageSize > totalcount)
                throw new Exception("invalid page size");

            int skip = (pageNo -  1) * pageSize;

            var response = query
                .Skip(skip)
                .Take(pageSize)
                .Select(r => new RoadmapResponseDto()
                {
                    RoadmapName = r.Name,
                    RoadmapDescription = r.Description
                }).ToList();
           return new PagedResult<RoadmapResponseDto>()
           {
               Items = response,
               Page = pageNo,
               PageSize = pageSize,
               TotalCount = totalcount
           };
        }

        public RoadmapDetailsResponse GetRoadmapDetails(int id)
        {
            var query = _roadmapRepo.GetByIdWithDetails(id);

            if (query == null)
                throw new Exception("not found");
           

            RoadmapDetailsResponse roadmapDetails = new RoadmapDetailsResponse();
            roadmapDetails.roadmapName = query.Name;
            roadmapDetails.trackName = query.Track?.Name;
            roadmapDetails.roadmapDescription = query.Description;
            roadmapDetails.avarageRating = 
                query.Reviews.Any() ? query.Reviews.Average(r => r.Rate) : 0;
            roadmapDetails.levelResoponses = query.Levels.Select(l => new LevelResoponse()
            {
                levelName = l.Name,
                topicResponses = l.Topics.OrderBy(t => t.Order)
                .Select(t => new TopicResponse()
                {
                    topicName = t.Name,
                    topicOrder = t.Order,
                    resources = t.Resources.OrderBy(r => r.Order)
                    .Select(r => new ResourceResponse()
                    {
                        resourceName = r.Name,
                        resourceOrder = r.Order,
                        resourceType = r.Type,
                        rsourceUrl = r.ResUrl,
                        IsPaid = r.Paid
                    }).ToList()
                }).ToList()
            });

            return roadmapDetails;

        }

        public void UpdateRoadmap(int id, RoadmapRequestDto roadmapDto)
        {
            var roadmap = _roadmapRepo.GetById(id);
            if (roadmap == null)
            {
                throw new Exception("not found");
            }

            if (roadmap.Name == roadmapDto.RoadmapName && roadmap.TrackId == roadmapDto.trackId)
                return;

            roadmap.Name = roadmapDto.RoadmapName;
            roadmap.Description = roadmapDto.RoadmapDescription;
            roadmap.TrackId = roadmapDto.trackId;

            bool isExisted = _roadmapRepo.IsExist(roadmap);
            if (isExisted)
            {
                throw new Exception("roadmap already existed");
            }

            _roadmapRepo.Update(roadmap);

            try
            {
                _roadmapRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed : {ex.Message}");
            }
        }
    }
}
