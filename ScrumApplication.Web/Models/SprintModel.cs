using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumApplication.Web.Models
{
    public class SprintModel
    {
        public int SprintId { get; set; }
        public string Name { get; set; }
        public DateTime EndDate { get; set; }
        public int ProjectId { get; set; }
        public List<ProductBacklog> ProductBacklogs { get; set; }
        public List<Epic> Epics { get; set; }
    }
}