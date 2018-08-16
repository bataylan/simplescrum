﻿using System;
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
        public int DayCount { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public int DefaultSprintTime { get; set; }
        public int CurrentSprintNo { get; set; }
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
