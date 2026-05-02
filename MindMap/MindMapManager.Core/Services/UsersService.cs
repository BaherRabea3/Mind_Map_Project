using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.Exceptions;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;

namespace MindMapManager.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserTrackRepository _userTrackRepo;
        private readonly ICompletedTopicRepository _completedTopicRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersService(IUserTrackRepository userTrackRepo, ICompletedTopicRepository completedTopicRepo, UserManager<ApplicationUser> userManager)
        {
            _userTrackRepo = userTrackRepo;
            _completedTopicRepo = completedTopicRepo;
           _userManager = userManager;
        }

        public async Task<UserProfileResponse> GetUserDetails(int userId)
        {
           var user = await  _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("user not found");
            }
           var response = new UserProfileResponse()
           {
               Id = userId,
               UserName = user.UserName,
               FullName = user.FullName,
               Email = user.Email,
               status = user.Status,
               Streak = user.Streak!.Value,
               IsVerified = user.IsVerified ?? false
           };
            return response;
        }

        public IEnumerable<TrackProgressResponse> GetUserProgress(int userid)
        {
            var userTracks = _userTrackRepo.GetAllUserTracks(userid);

            if(!userTracks.Any())
                return Enumerable.Empty<TrackProgressResponse>();

            var rawData = userTracks
                .Select(ut => new
                {
                    TrackId = ut.trackId,
                    TrackName = ut.Track.Name,
                    Roadmaps = ut.Track.Roadmaps.Select(r => new
                    {
                        r.Rid,
                        r.Name,
                        r.Description,
                        Levels = r.Levels.Select(l => new
                        {
                            l.Lid,
                            ProgressValues = l.Progresses
                                .Where(p => p.UserId == userid)
                                .Select(p => p.CompPerc)
                        })
                    })
                })
                .ToList();

            var result = rawData.Select(t => new TrackProgressResponse
            {
                trackId = t.TrackId,
                trackName = t.TrackName,

                Roadmaps = t.Roadmaps.Select(r => new RoadmapProgressResponse
                {
                    roadmapId = r.Rid,
                    roadmapName = r.Name,
                    roadmapDescription = r.Description,
                    lastTopicCompleted = _completedTopicRepo.GetLastTopicCompleted(userid,r.Rid) ?? "no completed topic yet",
                    Percentage = (int)Math.Round(
                        r.Levels
                            .SelectMany(l => l.ProgressValues.DefaultIfEmpty(0))
                            .DefaultIfEmpty(0)
                            .Average()
                    ),

                    Levels = r.Levels.Select(l => new LevelProgressResponse
                    {
                        levelId = l.Lid,
                        progressPercentage = (int)l.ProgressValues.FirstOrDefault()
                    }).ToList()

                }).ToList()

            }).ToList();

            return result;
        }

        public async Task UpdateProfile(int userId , UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("user not found");
            }

            var fullName = string.IsNullOrWhiteSpace(request.FullName)
                ? user.FullName : request.FullName.Trim();

            if (user.FullName.Trim() == fullName)
                return;

            user.FullName = fullName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var msg = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new BadRequestException(msg);
            }
        }
    }
}
