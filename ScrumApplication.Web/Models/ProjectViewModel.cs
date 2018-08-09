using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumApplication.Entity.Models;

namespace ScrumApplication.Web.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public List<Team> UserTeams { get; set; }

    }
}