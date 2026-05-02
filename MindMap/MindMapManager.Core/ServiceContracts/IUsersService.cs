using MindMapManager.Core.DTOs;
using MindMapManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.ServiceContracts
{
    public interface IUsersService
    {
        IEnumerable<TrackProgressResponse> GetUserProgress(int userid);
        Task<UserProfileResponse> GetUserDetails(int userId);
        Task UpdateProfile(int userId , UpdateProfileRequest request);
    }
}
