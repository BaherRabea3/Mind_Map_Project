using Microsoft.EntityFrameworkCore;
using MindMapManager.Core.Entities;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.DatabaseContext;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Infrastructure.Repository
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;

        public TopicRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Topic topic)
        {
            _context.Topics.Add(topic);
        }

        public int CountPerLevel(int levelId)
        {
            return _context.Topics.Count(x => x.Lid == levelId);
        }

        public void Delete(Topic topic)
        {
            _context.Topics.Remove(topic);
        }

        public Topic? GetById(int id)
        {
            return _context.Topics.FirstOrDefault(t => t.TopicId == id);
        }

        public Topic? GetByIdWithResources(int id)
        {
            return _context.Topics
                .Include(t => t.Resources)
                .FirstOrDefault(x => x.TopicId == id);
        }

        public Topic? GetWithLevel(int id)
        {
            return _context.Topics.Include(x => x.LidNavigation)
                .FirstOrDefault(x => x.TopicId ==id);
        }

        public bool IsExist(Topic topic)
        {
            return _context.Topics
                .Any(t => t.Name == topic.Name && t.Lid == topic.Lid);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Topic topic)
        {
           _context.Update(topic);
        }
    }
}
