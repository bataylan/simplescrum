using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;

namespace ScrumApplication.DAL.Repositories
{
    public class ProductBacklogRepository
    {
        public static void AddProductBacklog(int projectId, string name)
        {
            var newTask = new ProductBacklog();
            newTask.Name = name;
            newTask.ProjectId = projectId;
            using (var db = new ScrumApplicationDbContext())
            {
                db.ProductBacklogs.Add(newTask);
                db.SaveChanges();
            }
            ActivityRepository.ActivityCreator
                    ("added " + newTask.Name+" to the project.", newTask.ProjectId, newTask.ProductBacklogId);
        }

        public static List<ProductBacklog> GetProductBacklogs(int projectId)
        {
            List<ProductBacklog> ProductBacklogs = new List<ProductBacklog>();

            using (var db = new ScrumApplicationDbContext())
            {
                var query = from task in db.ProductBacklogs
                            where task.ProjectId == projectId
                            orderby task.Priority ascending
                            select task;
                ProductBacklogs = query.ToList();
            }

            return ProductBacklogs;
        }

    }
}
