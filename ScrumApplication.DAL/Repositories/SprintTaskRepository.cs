using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;

namespace ScrumApplication.DAL.Repositories
{
    public class SprintTaskRepository
    {
        public static void AddSprintTask(int sprintId, string name)
        {
            var newTask = new SprintTask();
            newTask.Name = name;
            newTask.SprintId = sprintId;
            using (var db = new ScrumApplicationDbContext())
            {
                db.SprintTasks.Add(newTask);
                db.SaveChanges();
            }
        }



        public static List<SprintTask> GetSprintTasks(int sprintId)
        {
            List<SprintTask> sprintTasks = new List<SprintTask>();

            using (var db = new ScrumApplicationDbContext())
            {
                var query = from task in db.SprintTasks
                            where task.SprintId == sprintId
                            orderby task.Priority ascending
                            select task;
                sprintTasks = query.ToList();
            }

            return sprintTasks;
        }
    }
}
