using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string ActivityDescription { get; set; }
        public DateTime CreatedTime { get; set; }

        public int? ProjectId { get; set; }
        public int? BacklogId { get; set; }
        public int? MemberId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProductBacklog ProductBacklog { get; set; }
        public virtual Member Member { get; set; }

        public Activity()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
