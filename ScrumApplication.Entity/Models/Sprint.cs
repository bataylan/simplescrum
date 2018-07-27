using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Sprint
    {
        public int SprintId { get; set; }
        public string Name { get; set; }
        public int DefaultTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDone { get; set; }

        public int ProjectId { get; set; }

        public virtual List<SprintTask> SprintTasks { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<ProjectTask> ProjectTasks { get; set; }

        public Sprint()
        {
            IsDone = false;
            DefaultTime = 15;
            StartDate = DateTime.Now.Date;
            EndDate = StartDate.AddDays(DefaultTime);
        }
    }
}
