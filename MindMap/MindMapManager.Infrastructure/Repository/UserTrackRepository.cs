using Microsoft.EntityFrameworkCore;
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
    public class UserTrackRepository : IUserTrackRepository
    {
        private readonly AppDbContext _context;

        public UserTrackRepository(AppDbContext context)
        {
            _context = context;
        }

        public void add(UserTrack userTrack)
        {
            _context.Add(userTrack);
        }

        public IQueryable<UserTrack> GetAllUserTracks(int userId)
        {
            return _context.UserTracks
                 .Include(ut => ut.Track)
                     .ThenInclude(t => t.Roadmaps)
                         .ThenInclude(r => r.Levels)
                  .Where(ut => ut.userId == userId);
        }

        public bool IsEnrolled(int userId, int? trackId)
        {
            return _context.UserTracks
                .Any(ut => ut.trackId == trackId && ut.userId == userId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
