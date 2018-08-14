using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class ProductBacklog
    {
        [Key]
        public int ProductBacklogId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int Priority { get; set; }
        public int StoryPoint { get; set; }
        public bool Done { get; set; }
        public Status BacklogStatus { get; set; }
        public string AcceptanceCriteria { get; set; }
        public int SprintNo { get; set; }
        public string EpicName { get; set; }

        public int ProjectId { get; set; }

        public int? EpicId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Epic Epic { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

        public ProductBacklog()
        {
            Priority = 5;
            SprintNo = 1;
            StoryPoint = 1;
            Done = false;
            AcceptanceCriteria = null;
        }
    }
    public enum Status
    {
        Open,
        Approved,
        Developing,
        Completed,
        Tested,
        Pending,
        Cancelled
    }

    public enum BacklogSort
    {
        PriorityAsc,
        PriorityDesc,
        SprintAsc,
        SprintDesc,
        StoryPointAsc,
        StoryPointDesc
    }

}
