
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Helpers;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ITrackService
    {
        public IEnumerable<TrackResponse> GetTracksWithMostEnrollments(int amount);
        public PagedResult<TrackResponse> GetAll(int page, int pageSize, string? SearchTirm);
        public void AddTrack(TrackRequestDto trackDto);
        public void DeleteTrack(int id);
        public void UpdateTrack(int id , TrackRequestDto trackDto);
        public PagedResult<RoadmapResponseDto> GetRoadmapsByTrackId(int trackId, int pageNo, int pageSize);
    }
}
