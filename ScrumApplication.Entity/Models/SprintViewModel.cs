using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class SprintViewModel
    {
        public int ProjectId { get; set; }
        public int ProjectCurrentSprint { get; set; }
        public int SprintCount { get; set; }
        public List<ProductBacklog> SprintBacklogs { get; set; }
        public int SprintNo { get; set; }


        public SprintViewModel()
        {
            ProjectId = 0;
            SprintBacklogs = new List<ProductBacklog>();
            ProjectCurrentSprint = 0;
            SprintCount = 0;
        }
    }
}
