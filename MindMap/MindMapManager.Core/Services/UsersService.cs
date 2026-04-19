using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;


namespace MindMapManager.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserTrackRepository _userTrackRepo;
        private readonly ICompletedTopicRepository _completedTopicRepo;
        public UsersService(IUserTrackRepository userTrackRepo, ICompletedTopicRepository completedTopicRepo)
        {
            _userTrackRepo = userTrackRepo;
            _completedTopicRepo = completedTopicRepo;
        }

        public IEnumerable<TrackProgressResponse> GetUserProgress(int userid)
        {
            var userTracks = _userTrackRepo.GetAllUserTracks(userid);


            var rawData = _userTrackRepo.GetAllUserTracks(userid)
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

    }
}
