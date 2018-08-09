using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.DAL.Repositories
{
    public class EpicRepository
    {
        public void AddEpic(string name, int projectId)
        {
            var _newEpic = new Epic();
            _newEpic.Name = name;
            _newEpic.ProjectId = projectId;

            using (var db = new ScrumApplicationDbContext())
            {
                db.Epics.Add(_newEpic);
                db.SaveChanges();
            }
        }

        public static List<Epic> GetEpics(int projectId)
        {
            var _EpicList = new List<Epic>();

            using (var db = new ScrumApplicationDbContext())
            {
                if (projectId != 0)
                {
                    var query = from epic in db.Epics
                                where epic.ProjectId == projectId
                                orderby epic.IsDone ascending, epic.Priority ascending
                                select epic;
                    _EpicList = query.ToList();
                }
            }

            return _EpicList;
        }
    }
}
