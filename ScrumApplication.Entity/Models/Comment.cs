using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [DataType(DataType.MultilineText)]
        public string MemberComment { get; set; }
        public int MemberId { get; set; }
        public string MemberShortName { get; set; }
        public int ProductBacklogId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Member Member { get; set; }
        public virtual ProductBacklog ProductBacklog { get; set; }
    }
}
