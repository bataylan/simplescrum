using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class ProjectEditViewModel
    {
        public Project Project { get; set; }
        public List<Team> UserTeams { get; set; }
        public Team AssignedTeam { get; set; }

        public ProjectEditViewModel()
        {
            Project = new Project();
            UserTeams = new List<Team>();
            AssignedTeam = new Team();
        }
    }
}
