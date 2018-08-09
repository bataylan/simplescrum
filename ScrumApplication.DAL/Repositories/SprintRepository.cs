//using ScrumApplication.Entity.DbContext;
//using ScrumApplication.Entity.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ScrumApplication.DAL.Repositories
//{
//    public class SprintRepository
//    {
//        public static void AddSprint(string name, int projectId)
//        {
//            var _newSprint = new Sprint();
//            _newSprint.Name = name;
//            _newSprint.ProjectId = projectId;

//            using (var db = new ScrumApplicationDbContext())
//            {
//                db.Sprints.Add(_newSprint);
//                db.SaveChanges();
//            }
//        }
//        public static double GetRemainingTime(int sprintId)
//        {
//            double remainingTime = 0;
//            using (var db = new ScrumApplicationDbContext())
//            {
//                var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == sprintId);
//                var startDate = existSprint.StartDate;

//                if (existSprint.EndDate.Subtract(existSprint.StartDate).TotalMinutes > 0.0)
//                {
//                    remainingTime = existSprint.EndDate.Subtract(existSprint.StartDate).TotalDays;
//                    return remainingTime;
//                }
//                return 0;
//            }
//        }

//        public static SprintModel GetSprintModel(int projectId)
//        {
//            using (var db = new ScrumApplicationDbContext())
//            {
//                int sprintId = SprintRepository.GetActiveSprint(projectId);
//                var sprint = new Sprint();

//                sprint = db.Sprints.FirstOrDefault(x => x.SprintId == sprintId);
//                var newModel = new SprintModel();
//                newModel.ProjectId = projectId;
//                var ProductBacklogs = new List<ProductBacklog>();
//                var sprintPTasks = new List<Epic>();
//                ProductBacklogs = sprint.ProductBacklogs.ToList();
//                sprintPTasks = sprint.Epics.ToList();
//                if (ProductBacklogs != null)
//                {
//                    var query = from task in ProductBacklogs
//                                orderby task.Done ascending, task.Priority ascending
//                                select task;
//                    newModel.ProductBacklogs = query.ToList();
//                }
//                if (sprintPTasks != null)
//                {
//                    var nquery = from task in sprintPTasks
//                                 orderby task.IsDone ascending, task.Priority ascending
//                                 select task;
//                    newModel.Epics = nquery.ToList();
//                }
//                newModel.SprintId = sprint.SprintId;
//                newModel.Name = sprint.Name;
//                newModel.EndDate = sprint.EndDate;

//                return newModel;
//            }

//        }

//        public static int GetActiveSprint(int id)
//        {
//            using (var db = new ScrumApplicationDbContext())
//            {
//                foreach(var sprint in db.Sprints)
//                {
//                    if (sprint.ProjectId==id && sprint != null && !sprint.IsDone)
//                    {
//                        return sprint.SprintId;
//                    }
//                }

//                return CreateSprint(id);
//            }
                
//        }

//        public static int CreateSprint (int projectId)
//        {
//            using (var db = new ScrumApplicationDbContext())
//            {
//                var newSprint = new Sprint();
//                int sprintCount = 0;
//                var existProject = new Project();
//                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
//                if(!existProject.IsDone)
//                {
//                    foreach (var sprint in db.Sprints)
//                    {
//                        if (sprint.ProjectId == projectId)
//                        {
//                            sprintCount++;
//                        }
//                    }
//                    var sprintProject = new Project();
//                    sprintProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
//                    newSprint.Name = "Sprint " + sprintCount;
//                    newSprint.ProjectId = sprintProject.ProjectId;
//                    newSprint.DefaultTime = sprintProject.DefaultSprintTime;
//                    newSprint.EndDate = newSprint.StartDate.AddDays(newSprint.DefaultTime);

//                    db.Sprints.Add(newSprint);
//                    db.SaveChanges();

//                    return newSprint.SprintId;
//                }

//                return 0;
//            }
                
//        }

//        public static List<ProductBacklog> GetProductBacklogs(int sprintId)
//        {
//            var _taskList = new List<ProductBacklog>();
//            using (var db = new ScrumApplicationDbContext())
//            {
//                if(sprintId != 0)
//                {
//                    var query = from task in db.ProductBacklogs
//                                where task.SprintId == sprintId
//                                orderby task.Priority ascending
//                                select task;
//                    _taskList = query.ToList();
//                }
//                else
//                {
//                    var query = from task in db.ProductBacklogs
//                                orderby task.Priority ascending
//                                select task;
//                    _taskList = query.ToList();
//                }
//                return _taskList;
//            }
//        }
//    }
//}
