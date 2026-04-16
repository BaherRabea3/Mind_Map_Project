using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Helpers;


namespace MindMapManager.Core.ServiceContracts
{
    public interface IRoadmapService
    {
        public PagedResult<RoadmapResponseDto> GetAll(int pageNo, int pageSize, string? searchTirm);
        public RoadmapCreateResponse AddRoadmap(RoadmapRequestDto roadmapRequestDto);
        public RoadmapDetailsResponse GetRoadmapDetails(int id);
        public void DeleteRoadmap(int id);
        public void UpdateRoadmap(int id, RoadmapRequestDto roadmapDto);

    }
}
