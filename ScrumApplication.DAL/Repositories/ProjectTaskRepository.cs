using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.DAL.Repositories
{
    public class ProjectTaskRepository
    {
        public void AddProjectTask(string name, int projectId)
        {
            var _newProjectTask = new ProjectTask();
            _newProjectTask.Name = name;
            _newProjectTask.ProjectId = projectId;

            using (var db = new ScrumApplicationDbContext())
            {
                db.ProjectTasks.Add(_newProjectTask);
                db.SaveChanges();
            }
        }

        public static List<ProjectTask> GetProjectTasks(int projectId)
        {
            var _projectTaskList = new List<ProjectTask>();

            using (var db = new ScrumApplicationDbContext())
            {
                if (projectId != 0)
                {
                    var query = from projectTask in db.ProjectTasks
                                where projectTask.ProjectId == projectId
                                orderby projectTask.Priority ascending
                                select projectTask;
                    _projectTaskList = query.ToList();
                }
                else
                {
                    var query = from projectTask in db.ProjectTasks
                                orderby projectTask.Priority ascending
                                select projectTask;
                    _projectTaskList = query.ToList();
                }
            }

            return _projectTaskList;
        }
    }
}
