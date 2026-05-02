using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.Helpers;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;
using System.Data;


namespace MindMapManager.Core.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepo;
        private readonly IRoadmapRepository _roadmapRepo;
        private readonly IFileService _fileService;

        public TrackService(ITrackRepository trackRepo , IRoadmapRepository roadmapRepo, IFileService fileService)
        {
            _trackRepo = trackRepo;
            _roadmapRepo = roadmapRepo;
            _fileService = fileService;
        }

        public async Task AddTrack(TrackRequestDto trackDto)
        {
           var track = _trackRepo.FindByName(trackDto.TrackName);
            if (track != null)
            {
                throw new ConflictException("The track name is already exist");
            }

            var trackName = trackDto.TrackName?.Trim();
            if (string.IsNullOrWhiteSpace(trackName))
                throw new BadRequestException("track name is required");
            var trackDescription = trackDto.TrackDescription.Trim();
            if (string.IsNullOrWhiteSpace(trackDescription))
                throw new BadRequestException("track description is required");
            string? ImagePath = null;
            try
            {
                string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".webp" };

                ImagePath = await _fileService.SaveFileAsync(trackDto.TrackImage, allowedExtensions, "tracks");

                Track Track = new Track();
                Track.Name = trackName;
                Track.Description = trackDescription;
                Track.IconUrl = ImagePath;

                _trackRepo.Add(Track);

                _trackRepo.Save();
            }
            catch
            {
                if(!string.IsNullOrEmpty(ImagePath))
                    _fileService.DeleteFile(ImagePath);
                throw;
            }
        }

        public void DeleteTrack(int id)
        {
            Track? track = _trackRepo.GetById(id);

            if (track == null)
            {
                throw new NotFoundException("not found");
            }
            var ImagePath = track.IconUrl;
            _trackRepo.Remove(track);

            _trackRepo.Save();

            if(!string.IsNullOrEmpty(ImagePath))
                _fileService.DeleteFile(ImagePath);
        }

        public PagedResult<TrackResponse> GetAll(int page, int pageSize, string? SearchTirm)
        {
            if (page < 1 || pageSize < 1)
                throw new BadRequestException ("Page number must be greater than 0" );

            var query = _trackRepo.Search(SearchTirm);
            int totalCount = query.Count();
            var totaPages = Math.Ceiling(Convert.ToDecimal(totalCount) / pageSize);

            if (page > totaPages)
               throw new BadRequestException("Invalid Page number");

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
                    EnrollmentsCount = t.UserTracks.Count(),
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
                throw new BadRequestException("Invalid Tracks number");
            }
            var Tracks = _trackRepo.Get()
                .OrderByDescending(t => t.UserTracks.Count())
                .Take(Tracksnumber)
                .Select(t => new TrackResponse
                {
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                    TrackDescription = t.Description,
                    TrackIcon = t.IconUrl,
                    EnrollmentsCount = t.UserTracks.Count(),
                    RoadmapsCount = t.Roadmaps.Count()
                }).ToList();

            return Tracks;
        }

        public async Task UpdateTrack(int id, UpdateTrackRequestDto trackDto)
        {
            Track? track = _trackRepo.GetById(id);

            if (track == null)
                throw new NotFoundException("track is not found");

            string? oldImagePath = track.IconUrl;
            string? newImagePath = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(trackDto.TrackName))
                    track.Name = trackDto.TrackName.Trim();
                if (!string.IsNullOrWhiteSpace(trackDto.TrackDescription))
                    track.Description = trackDto.TrackDescription.Trim();
                if (trackDto.TrackImage != null)
                {
                    string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".webp" };
                    newImagePath = await _fileService.SaveFileAsync(trackDto.TrackImage, allowedExtensions, "tracks");
                    track.IconUrl = newImagePath;
                }

                _trackRepo.Update(track);
                _trackRepo.Save();
            }
            catch
            {
                if(!string.IsNullOrEmpty(newImagePath))
                    _fileService.DeleteFile(newImagePath);

                throw;
            }
            if (!string.IsNullOrEmpty(newImagePath) && !string.IsNullOrEmpty(oldImagePath))
            {
                _fileService.DeleteFile(oldImagePath);
            }
        }

        public PagedResult<RoadmapResponseDto> GetRoadmapsByTrackId(int trackId, int pageNo, int pageSize)
        {
            if (pageNo < 1 || pageSize < 1)
                throw new BadRequestException("invalid pageNo or page size");

            var track = _trackRepo.GetById(trackId);
            if (track == null)
                throw new NotFoundException("track not found");

            var query = _roadmapRepo.FilterByTrackId(trackId);
            var totalcount = query.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalcount / pageSize);

            if (pageNo > totalPages)
                throw new BadRequestException("invalid pageNo");

            int skip = (pageNo - 1) * pageSize;

            var response = query
                .Skip(skip)
                .Take(pageSize)
                .Select(r => new RoadmapResponseDto()
                {
                    RoadmapId = r.Rid,
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
    }
}
