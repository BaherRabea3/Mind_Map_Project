using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindMapManager.Core.DTOs
{
    public class UserProfileResponse
    {
        public int Id {  get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string status { get; set; } = null!;
        public bool IsVerified { get; set; }
        public int Streak { get; set; }
    }
}
