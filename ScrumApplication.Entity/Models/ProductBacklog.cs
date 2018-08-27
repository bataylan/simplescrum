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
        [Display(Name = "Story Point")]
        public int StoryPoint { get; set; }
        [Display(Name = "Is Done")]
        public bool Done { get; set; }
        [Display(Name = "Backlog Status")]
        public Status BacklogStatus { get; set; }
        [Display(Name = "Acceptence Criteria")]
        public string AcceptanceCriteria { get; set; }
        [Display(Name = "Sprint No")]
        public int SprintNo { get; set; }
        [Display(Name = "Epic Name")]
        public string EpicName { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Display(Name = "Epic")]
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
