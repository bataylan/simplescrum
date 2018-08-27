using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Day Count")]
        public int DayCount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Sprint Period")]
        public int DefaultSprintTime { get; set; }

        [Display(Name = "Active Sprint No")]
        public int CurrentSprintNo { get; set; }

        [Display(Name = "Is Done")]
        public bool IsDone { get; set; }

        public int? TeamId { get; set; }
        

        public virtual ICollection<ProductBacklog> ProductBacklogs { get; set; }
        public virtual ICollection<Epic> Epics { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

        public Project ()
        {
            DefaultSprintTime = 15;
            CurrentSprintNo = 1;
            DayCount = 60;
            CreatedDate = DateTime.Now.Date;
            EndDate = CreatedDate.AddDays(DayCount);
            IsDone = false;
        }
    }
}
