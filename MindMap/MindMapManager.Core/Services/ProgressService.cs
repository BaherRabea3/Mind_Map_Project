using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Core.ServiceContracts;


namespace MindMapManager.Core.Services
{
    public class ProgressService : IProgressService
    {
        private readonly IProgressRepository _progressRepo;
        private readonly ITopicRepository _topicRepo;
        private readonly ICompletedTopicRepository _completedTopicRepo;
        private readonly IRoadmapRepository _roadmapRepo;
        private readonly IUserTrackRepository _userTrackRepo;

        public ProgressService(IProgressRepository progressRepo, ITopicRepository topicRepo, ICompletedTopicRepository completedTopicRepo,IRoadmapRepository roadmapRepo, IUserTrackRepository userTrackRepo)
        {
            _progressRepo = progressRepo;
            _topicRepo = topicRepo;
            _completedTopicRepo = completedTopicRepo;
            _roadmapRepo = roadmapRepo;
            _userTrackRepo = userTrackRepo;
        }

        public void CompleteTopic(int userId, int topicId)
        {
            var topic = _topicRepo.GetWithLevel(topicId);

            if (topic == null)
            {
                throw new Exception("topic not found");
            }
            var levelId = topic.Lid.Value;
            

            var isCompleted = _completedTopicRepo.IsCompleted(userId, topicId);

            if (isCompleted)
            {
                return;
            }

            _completedTopicRepo.Add(new CompletedTopic()
            {
                topicId = topicId,
                userId = userId,
            });

            _completedTopicRepo.Save();

            var totalTopics = _topicRepo.CountPerLevel(levelId);
            var completedTopics = _completedTopicRepo.CountPerUserInLevel(userId, levelId);

            decimal percentage = Math.Round(((decimal)completedTopics/ totalTopics)*100);

            var progress = _progressRepo.GetProgress(userId, levelId);

            if (progress == null)
            {
                throw new Exception("user doesn't enroll in the track");
            }

            progress.CompPerc = percentage;

            _progressRepo.Update(progress);

            _progressRepo.Save();
        }

        public RoadmapProgressResponse RoadmapProgress(int userId ,int roadmapId)
        {
            var roadmap = _roadmapRepo.GetByIdWithLevelsAndProgress(roadmapId);

            if (roadmap == null)
                throw new Exception("not found");

            bool isEnrolled = _userTrackRepo.IsEnrolled(userId, roadmap.TrackId);
            if (!isEnrolled)
            {
                throw new Exception("user doesn't enroll in the track");
            }

            var lastTopicName = _completedTopicRepo.GetLastTopicCompleted(userId, roadmapId);

            if (lastTopicName == null)
                lastTopicName = "no topic yet";

            return new RoadmapProgressResponse()
            {
                roadmapName = roadmap.Name,
                roadmapDescription = roadmap.Description,
                lastTopicCompleted = lastTopicName,
                Percentage = (int)Math.Round(
                    roadmap.Levels
                    .Select(l => l.Progresses.
                    Where(p => p.UserId == userId)
                    .Select(p => p.CompPerc)
                    .FirstOrDefault())
                    .Average())
            };
        }
    }
}
