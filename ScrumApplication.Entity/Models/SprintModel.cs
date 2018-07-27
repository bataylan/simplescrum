using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumApplication.Entity.Models
{
    public class SprintModel
    {
        public int SprintId { get; set; }
        public string Name { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public List<SprintTask> SprintTasks { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
    }
}