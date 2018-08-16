using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class ProjectHomeViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int TeamId { get; set; }
        public List<ProductBacklog> ProductBacklogs { get; set; }
        public int CurrentSprintNo { get; set; }
        public int TotalSprintCount { get; set; }
        public int ViewSprintNo { get; set; }
        public int ViewSortBy { get; set; }
    }
}
