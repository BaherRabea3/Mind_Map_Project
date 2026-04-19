using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Infrastructure.Repository
{
    public class CompletedTopicRepository : ICompletedTopicRepository
    {
        private readonly AppDbContext _context;

        public CompletedTopicRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(CompletedTopic completedTopic)
        {
            _context.Add(completedTopic);
        }

        public int CountPerUserInLevel(int userId, int levelId)
        {
           return _context.CompletedTopics.Count(x => x.userId == userId && x.Topic.Lid == levelId);
        }

        public string? GetLastTopicCompleted(int userId, int roadmapId)
        {
            return _context.CompletedTopics
                .Where(x => x.userId == userId)
                .Where(x => x.Topic.LidNavigation.Rid == roadmapId)
                .OrderByDescending(x => x.Id)
                .Select(x => x.Topic.Name)
                .FirstOrDefault();
        }

        public bool IsCompleted(int userId, int topicId)
        {
            return _context.CompletedTopics
                .Any(x => x.userId == userId && x.topicId == topicId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
