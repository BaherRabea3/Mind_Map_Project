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
    public class ProgressRepository : IProgressRepository
    {
        private readonly AppDbContext _context;

        public ProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Progress progress)
        {
           _context.Add(progress);
        }

        public Progress? GetProgress(int userId, int levelId)
        {
            return _context.Progresses
                .FirstOrDefault(x => x.UserId == userId && x.Lid == levelId);
        }

        public void Update(Progress progress)
        {
            _context.Update(progress);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
