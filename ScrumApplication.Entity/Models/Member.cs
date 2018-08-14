using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        public int RoleCode { get; set; }
        public int TotalPoint { get; set; }

        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int? CompanyId { get; set; }

        public virtual User User { get; set; }
        public virtual Team Team { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<ProductBacklog> ProductBacklogs { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }

    }
}
