using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class TeamCreateViewModel
    {
        public Team Team { get; set; }
        public int ProjectId { get; set; }

        public TeamCreateViewModel()
        {
            Team = new Team();
            ProjectId = 0;
        }
    }
}
