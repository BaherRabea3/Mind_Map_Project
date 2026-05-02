
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
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
                throw new NotFoundException("invalid track id");
            }

            var roadmapName = roadmapDto.RoadmapName.Trim();
            if (string.IsNullOrWhiteSpace(roadmapName))
                throw new BadRequestException("Roadmap name is required");

            var exists = _roadmapRepo.Existed(roadmapName, roadmapDto.trackId, null);

            if (exists)
            {
                throw new ConflictException("roadmap already existed");
            }

            var roadmap = new Roadmap();
            roadmap.Name = roadmapDto.RoadmapName;
            roadmap.TrackId = roadmapDto.trackId;
            roadmap.Description = roadmapDto.RoadmapDescription;

            _roadmapRepo.Add(roadmap);
            _roadmapRepo.Save();

            return new RoadmapCreateResponse()
            {
                id = roadmap.Rid,
                description = roadmap.Description,
                name = roadmap.Name,
                trackName = track.Name,
            };

        }

        public void DeleteRoadmap(int id)
        {
            var roadmap = _roadmapRepo.GetById(id);
            if (roadmap == null)
                throw new NotFoundException("roadmap not found");

            _roadmapRepo.Remove(roadmap);
            _roadmapRepo.Save();
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
                    RoadmapId  = r.Rid,
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
                throw new NotFoundException("roadmap not found");
           
            RoadmapDetailsResponse roadmapDetails = new RoadmapDetailsResponse();
            roadmapDetails.roadmapId = query.Rid;
            roadmapDetails.roadmapName = query.Name;
            roadmapDetails.trackName = query.Track?.Name;
            roadmapDetails.roadmapDescription = query.Description;
            roadmapDetails.avarageRating = query.Reviews.Any() ? query.Reviews.Average(r => r.Rate) : 0;
            roadmapDetails.levelResoponses = query.Levels.Select(l => new LevelResoponse()
            {
                levelId = l.Lid,
                levelName = l.Name,
                topicResponses = l.Topics.OrderBy(t => t.Order)
                .Select(t => new TopicResponse()
                {
                    topicId = t.TopicId,
                    topicName = t.Name,
                    topicOrder = t.Order,
                    resources = t.Resources.OrderBy(r => r.Order)
                    .Select(r => new ResourceResponse()
                    {
                        resourceId = r.ResId,
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
                throw new NotFoundException("roadmap not found");
            }
            var newName = string.IsNullOrWhiteSpace(roadmapDto.RoadmapName) 
                ? roadmap.Name : roadmapDto.RoadmapName.Trim();
            var newDescription = string.IsNullOrWhiteSpace (roadmapDto.RoadmapDescription)
                ? roadmap.Description : roadmapDto.RoadmapDescription.Trim();

            if (roadmap.Name == newName 
                && roadmap.Description == newDescription
                && roadmap.TrackId == roadmapDto.trackId)
                return;

            var exists = _roadmapRepo.Existed(newName, roadmapDto.trackId, id);
            if (exists)
            {
                throw new ConflictException("roadmap already existed");
            }

            roadmap.Name = roadmapDto.RoadmapName;
            roadmap.Description = roadmapDto.RoadmapDescription;
            roadmap.TrackId = roadmapDto.trackId;

            _roadmapRepo.Update(roadmap);
            _roadmapRepo.Save();
        }
    }
}
