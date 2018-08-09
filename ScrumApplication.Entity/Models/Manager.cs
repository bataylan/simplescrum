using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }

        public int UserId { get; set; }
        public int? MemberId { get; set; }
        public int? CompanyId { get; set; }

        
        public virtual User User { get; set; } 
        public virtual Member Member { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
