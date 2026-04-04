using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;
using System.Data;


namespace MindMapManager.Core.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepo;

        public TrackService(ITrackRepository trackRepo,UserManager<ApplicationUser> userManager)
        {
            _trackRepo = trackRepo;
        }

        public void AddTrack(TrackRequestDto trackDto)
        {
           var track = _trackRepo.FindByName(trackDto.TrackName);
            if (track != null)
            {
                throw new Exception("The track is already exist");
            }
            Track Track = new Track();
            Track.Name = trackDto.TrackName;
            Track.Description = trackDto.TrackDescription;
            Track.IconUrl = trackDto.TrackImage;

            _trackRepo.Add(Track);

            try
            {
                _trackRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add track");
            }
        }

        public void DeleteTrack(int id)
        {
            Track? track = _trackRepo.GetById(id);

            if (track == null)
            {
                throw new Exception("not found");
            }
            _trackRepo.Remove(track);

            try
            {
                _trackRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete track");
            }
        }

        public PagedResult<TrackResponse> GetAll(int page, int pageSize, string? SearchTirm)
        {
            if (page < 1 || pageSize < 1)
                throw new Exception ("Page number must be greater than 0" );

            var query = _trackRepo.Search(SearchTirm);
            int totalCount = query.Count();
            var totaPages = Math.Ceiling(Convert.ToDecimal(totalCount) / pageSize);

            if (page > totaPages)
               throw new Exception("Invalid Page number");

            int skipped = (page - 1) * pageSize;
            int available = totalCount - skipped;
            var taken = pageSize <= available ? pageSize : available;

            var TracksList = query
                .Skip(skipped)
                .Take(taken)
                .Select(t => new TrackResponse()
                {
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                    TrackDescription = t.Description,
                    TrackIcon = t.IconUrl,
                    EnrollmentsCount = t.Users.Count(),
                    RoadmapsCount = t.Roadmaps.Count()
                }).ToList();
            return new PagedResult<TrackResponse>()
            {
                Items = TracksList,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public IEnumerable<TrackResponse> GetTracksWithMostEnrollments(int Tracksnumber)
        {
            if (Tracksnumber <= 0 || Tracksnumber > _trackRepo.Count())
            {
                throw new Exception("Invalid Tracks number");
            }
            var Tracks = _trackRepo.Get()
                .OrderByDescending(t => t.Users.Count())
                .Take(Tracksnumber)
                .Select(t => new TrackResponse
                {
                    TrackName = t.Name,
                    TrackDescription = t.Description,
                    TrackIcon = t.IconUrl,
                    EnrollmentsCount = t.Users.Count(),
                    RoadmapsCount = t.Roadmaps.Count()
                }).ToList();

            return Tracks;
        }

        public void UpdateTrack(int id, TrackRequestDto trackDto)
        {
            Track? track = _trackRepo.GetById(id);
            if (track == null)
            {
                throw new Exception("not found");
            }

            track.Name = trackDto.TrackName;
            track.Description = trackDto.TrackDescription;
            track.IconUrl = trackDto.TrackImage;

            _trackRepo.Update(track);

            try
            {
                _trackRepo.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update track");
            }
        }
    }
}
