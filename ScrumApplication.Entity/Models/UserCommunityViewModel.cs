using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class UserCommunityViewModel
    {
        public Member Member { get; set; }
        public List<User> UserCommunity { get; set; }
        
        public UserCommunityViewModel()
        {
            Member = new Member();
        }
    }
}
