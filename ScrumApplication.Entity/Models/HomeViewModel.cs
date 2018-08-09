using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class HomeViewModel
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public ICollection<HomeProjectViewModel> UserActiveProjects { get; set; }
        public ICollection<Team> UserTeams { get; set; }

       

    }
}
