using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class BacklogToMember
    {
        public int BacklogToMemberId { get; set; }
        public int ProductBacklogId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public virtual ProductBacklog ProductBacklog { get; set; }
        public virtual Member Member { get; set; }
    }
}
