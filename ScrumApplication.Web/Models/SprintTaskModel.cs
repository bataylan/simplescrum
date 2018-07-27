using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumApplication.Web.Models
{
    public class SprintTaskModel
    {
        
        public int SprintTaskId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Point { get; set; }
        public bool IsDone { get; set; }
        public int SprintId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }

        public SprintTaskModel()
        {
            UserId = null;
            UserName = null;
        }
    }
}