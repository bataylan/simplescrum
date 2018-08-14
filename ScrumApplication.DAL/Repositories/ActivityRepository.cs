using ScrumApplication.Entity.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumApplication.Entity.Models;

namespace ScrumApplication.DAL.Repositories
{
    public class ActivityRepository
    {
        public static void ActivityCreator(string ActivityString, int projectId, int? backlogId )
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var memberId = TeamRepository.GetUserMemberIdFromProjectId(projectId);
                var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                var newActivity = new Activity();
                newActivity.MemberId = memberId;
                newActivity.ProjectId = projectId;
                if(backlogId.HasValue)
                {
                    newActivity.BacklogId = backlogId;
                }
                newActivity.ActivityDescription = 
                    existMember.Name +" " + ActivityString;
                //" at time: " + DateTime.Now.Minute + "." + DateTime.Now.Hour + " - " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year
                db.Activities.Add(newActivity);
                db.SaveChanges();
            }
        }
    }
}
