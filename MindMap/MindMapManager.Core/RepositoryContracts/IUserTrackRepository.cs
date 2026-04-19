using MindMapManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.RepositoryContracts
{
    public interface IUserTrackRepository
    {
        void add(UserTrack userTrack);
        bool IsEnrolled(int userId, int? trackId);
        IQueryable<UserTrack> GetAllUserTracks(int userId);
        void Save();
    }
}
