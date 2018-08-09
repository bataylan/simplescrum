using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumApplication.Entity.Models;

namespace ScrumApplication.Web.Models
{
    public class TeamViewModel
    {
        public Team Team { get; set; }
        public List<User> UserCommunity { get; set; }
    }
}