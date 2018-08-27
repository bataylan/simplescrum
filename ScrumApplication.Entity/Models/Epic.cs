using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Epic
    {
        [Display(Name = "Epic")]
        public int EpicId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsDone { get; set; }

        public int ProjectId { get; set; }
        
        public virtual Project Project { get; set; }
        public virtual ICollection<ProductBacklog> ProductBacklogs { get; set; }

        public Epic()
        {
            IsDone = false;
            Priority = 5;
        }

    }
}
