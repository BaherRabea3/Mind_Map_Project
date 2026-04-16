using MindMapManager.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.ServiceContracts
{
    public interface ITopicService
    {
        public TopicResponse GetWithDetails(int id);
        public int AddTopic(TopicRequest topicRequest);
        public void UpdateTopic(int id , TopicRequest topicRequest);
        public void DeleteTopic(int id);
    }
}
