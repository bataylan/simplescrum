using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public int Name { get; set; }


        public virtual List<Team> Teams { get; set; }
        public virtual List<Member> Members { get; set; }
        public virtual List<Manager> Managers { get; set; }
    }
}
