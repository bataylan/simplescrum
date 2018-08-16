using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.Entity.Models
{
    public class BacklogViewModel
    {
        public ProductBacklog Backlog { get; set; }
        public Epic Epic { get; set; }
        public List<Epic> ProjectEpics { get; set; }
        public List<MemberViewModel> AssignedMembers { get; set; }
        public List<MemberViewModel> UnAssignedMembers { get; set; }
        public bool IsUserAssigned { get; set; }
        public MemberViewModel AssignMember { get; set; }
        public List<Comment> BacklogComments { get; set; }
        public int ViewSprintNo { get; set; }
        public int ViewSortBy { get; set; }


        public BacklogViewModel()
        {
            this.AssignedMembers = new List<MemberViewModel>();
            this.UnAssignedMembers = new List<MemberViewModel>();
            IsUserAssigned = false;
            var productBacklog = new ProductBacklog();
            this.Backlog = productBacklog;
            var epic = new Epic();
            this.Epic = epic;
            BacklogComments = new List<Comment>();
        }
    }
}
