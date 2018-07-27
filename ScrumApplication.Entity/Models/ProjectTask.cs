using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class ProjectTask
    {
        public int ProjectTaskId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsDone { get; set; }

        public int ProjectId { get; set; }
        public int? SprintId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Sprint Sprint { get; set; }

        public ProjectTask()
        {
            IsDone = false;
            Priority = 5;
        }

    }
}
