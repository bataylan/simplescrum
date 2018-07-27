using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        public string Name { get; set; }

        public int? ManagerId { get; set; }

        public int? CompanyId { get; set; }

        public virtual Manager Manager { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<Member> Members { get; set; }
        public virtual List<Project> Projects { get; set; }
    }
}
