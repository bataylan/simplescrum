using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class SprintTask
    {
        [Key]
        public int SprintTaskId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Point { get; set; }
        public bool Done { get; set; }
        public string AssignedTo { get; set; }

        public int SprintId { get; set; }

        public int? MemberId { get; set; }

        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Member Member { get; set; }
        public virtual Sprint Sprint { get; set; }

        public SprintTask()
        {
            Priority = 5;
            Point = 1;
            Done = false;
        }
    }

    
}
