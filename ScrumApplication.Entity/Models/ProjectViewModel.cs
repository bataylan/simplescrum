using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public List<Activity> ProjectActivities { get; set; }
        public List<Member> ProjectTeamMembers { get; set; }

        public ProjectViewModel()
        {
            Project = new Project();
            ProjectActivities = new List<Activity>();
            ProjectTeamMembers = new List<Member>();
        }
    }
}
