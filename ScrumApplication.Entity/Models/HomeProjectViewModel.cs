using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class HomeProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int MemberId { get; set; }
        public int RoleCode  { get; set; }
        public ICollection<ProductBacklog> AssignedActiveBacklogs { get; set; }


    }
}
