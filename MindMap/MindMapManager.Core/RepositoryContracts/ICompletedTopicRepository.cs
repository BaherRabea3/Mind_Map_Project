using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MindMapManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface ICompletedTopicRepository
    {
        int CountPerUserInLevel(int userId , int levelId);
        string? GetLastTopicCompleted(int userId , int roadmapId);
        bool IsCompleted(int userId, int topicId);
        void Add(CompletedTopic completedTopic);
        CompletedTopic? GetByUserAndTopic(int userId, int topicId);
        void Delete(int userId , int topicId);
        void Save();
    }
}
