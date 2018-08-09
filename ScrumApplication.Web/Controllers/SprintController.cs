//using ScrumApplication.DAL.Repositories;
//using ScrumApplication.Entity.DbContext;
//using ScrumApplication.Entity.Models;
//using ScrumApplication.Web.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace ScrumApplication.Web.Controllers
//{
//    public class SprintController : Controller
//    {
//        ScrumApplicationDbContext db;

//        public SprintController()
//        {
//            db = new ScrumApplicationDbContext();
//        }
//        // GET: Sprint
//        public ActionResult Index(int id)
//        {
//            var newModel = SprintRepository.GetSprintModel(id);

//            return View(newModel);
//        }

//        public ActionResult EndSprint(int id)
//        {
//            var existSprint = new Sprint();
//            var newSprint = new Sprint();
//            existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == id);
//            existSprint.IsDone = true;
//            var uncompletedTasks = new List<ProductBacklog>();
//            uncompletedTasks = existSprint.ProductBacklogs.ToList();
//            var uncompletedPTasks = new List<Epic>();
//            uncompletedPTasks = existSprint.Epics.ToList();
//            newSprint.SprintId = SprintRepository.CreateSprint(existSprint.ProjectId);
//            newSprint = db.Sprints.FirstOrDefault(x => x.SprintId == newSprint.SprintId);

//            if (uncompletedTasks != null)
//            {
//                var query = from task in uncompletedTasks
//                            where task.Done == false
//                            orderby task.Priority
//                            select task;

//                uncompletedTasks = query.ToList();

//                foreach (var task in uncompletedTasks)
//                {
//                    task.Sprint = newSprint;
//                    task.SprintId = newSprint.SprintId;
//                    newSprint.ProductBacklogs.Add(task);
//                }
//            }
//            else
//            {
//                foreach(var ptask in uncompletedPTasks)
//                {
//                    ptask.IsDone = true;
//                }
//            }
//            db.SaveChanges();

//            return RedirectToAction("Index", "Sprint", new { id = existSprint.ProjectId });
//        }

//        public ActionResult IndexEpic(int id)
//        {
//            List<Epic> Epics = new List<Epic>();
//            Epics = EpicRepository.GetEpics(id);
//            if(Epics.Count == 0)
//            {
//                var pTask = new Epic();
//                pTask.ProjectId = id;
//                Epics.Add(pTask);
//            }
//            return View(Epics);
//        }

//        //public ActionResult Edit(int id)
//        //{
//        //    var existSPTask = new ProductBacklog();
//        //    existSPTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);

//        //    return View(existSPTask);
//        //}

//        //[HttpPost]
//        //public ActionResult Edit(ProductBacklog existSPTask)
//        //{
//        //    var _existSPTask = new ProductBacklog();
//        //    _existSPTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == existSPTask.ProductBacklogId);
//        //    _existSPTask.Name = existSPTask.Name;
//        //    _existSPTask.Priority = existSPTask.Priority;
//        //    _existSPTask.Point = existSPTask.Point;
//        //    _existSPTask.Done = existSPTask.Done;
//        //    if(existSPTask.Done == false && _existSPTask.Done == true && existSPTask.UserId != null)
//        //    {
//        //        //Puanı ekle
//        //    }
                
//        //    else if(existSPTask.Done == true && _existSPTask.Done == false && existSPTask.UserId != null)
//        //    {
//        //        //Puanı düşür
//        //    }
//        //    //Eğer Taskın ilk hali done değilse ve son hali done'sa, kullanıcıya puan eklenecek
//        //    db.SaveChanges();
//        //    return RedirectToAction("Index", new { id = existSPTask.Sprint.ProjectId });
//        //}

//        public ActionResult CreateTask(int id)
//        {
//            var newTask = new ProductBacklog();
//            newTask.SprintId = id;
//            var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == id);
//            newTask.ProjectId = existSprint.ProjectId;
//            newTask.Sprint = existSprint;

//            return View(newTask);
//        }

//        [HttpPost]
//        public ActionResult CreateTask(ProductBacklog newTask)
//        {
//            var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == newTask.SprintId);
//            db.ProductBacklogs.Add(newTask);
//            //existSprint.ProductBacklogs.Add(newTask);
//            db.SaveChanges();

//            return RedirectToAction("Index", "Sprint", new { id = existSprint.ProjectId});
//        }

//        public ActionResult EditSprint(int id)
//        {
//            var existSprint = new Sprint();
//            existSprint = db.Sprints.FirstOrDefault(x=>x.SprintId == id);
//            return View(existSprint);
//        }

//        [HttpPost]
//        public ActionResult EditSprint(Sprint existSprint)
//        {
//            var _existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == existSprint.SprintId);
//            _existSprint.Name = existSprint.Name;
//            _existSprint.DefaultTime = existSprint.DefaultTime;
//            db.SaveChanges();
//            return View();
//        }

//        public ActionResult TakeIt(int id)
//        {
//            var existTask = new ProductBacklog();
//            existTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
//            if (UserRepository.IsUserSigned())
//            {
//                int teamId = TeamRepository.GetTeamIdFromProductBacklog(id);
//                if(teamId != 0)
//                {
//                    var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
//                    int userId = UserRepository.GetUserId();
//                    var existMember = existTeam.Members.ToList().FirstOrDefault(x => x.UserId == userId);
//                    if(existTeam.Manager.UserId == userId)
//                    {
//                        return Content("ScrumMaster can't take tasks");
//                    }
//                    if( existMember != null)
//                    {
//                        existTask.Member.Add(existMember);
//                        db.SaveChanges();

//                        return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId });
//                    }
//                }
                
//            }

//            return RedirectToAction("Login", "User");


//        }

//        public ActionResult GiveIn(int id)
//        {
//            var existTask = new ProductBacklog();
//            existTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
//            if (UserRepository.IsUserSigned())
//            {
//                int teamId = TeamRepository.GetTeamIdFromProductBacklog(id);
//                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
//                int userId = UserRepository.GetUserId();
//                var existMember = existTeam.Members.ToList().FirstOrDefault(x => x.UserId == userId);
//                existTask.Member.Remove(existMember);
//                db.SaveChanges();
//            }


//            return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId});

//        }

//        public ActionResult Green(int id)
//        {
//            var existTask = new ProductBacklog();
//            existTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
//            existTask.Done = false;
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId });
//        }

//        public ActionResult BackToList(int id)
//        {
//            if (UserRepository.IsUserSigned())
//            {

//                return RedirectToAction("Index", "Sprint", new { id = id });
//            }

//            return RedirectToAction("Login", "User");
//        }

//        public ActionResult CompleteEpic(int id)
//        {
//            var existTask = new Epic();
//            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
//            existTask.IsDone = true;
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
//        }

//        public ActionResult GreenEpic(int id)
//        {
//            var existTask = new Epic();
//            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
//            existTask.IsDone = false;
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId });
//        }

//        public ActionResult CreateEpic(int id)
//        {
//            var newEpic = new Epic();
//            newEpic.ProjectId = id;

//            return View(newEpic);
//        }

//        [HttpPost]
//        public ActionResult CreateEpic(Epic newEpic)
//        {
//            db.Epics.Add(newEpic);
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = newEpic.ProjectId});
//        }

//        public ActionResult Complete(int id)
//        {
//            if (UserRepository.IsUserSigned())
//            {
//                var existTask = new ProductBacklog();
//                int userId = UserRepository.GetUserId();
//                existTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
//                int teamId = TeamRepository.GetTeamIdFromProductBacklog(id);
//                if(teamId != 0)
//                {
//                    var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
//                    var existMember = existTeam.Members.ToList().FirstOrDefault(x => x.UserId == userId);
//                    if (existMember != null)
//                    {
//                        foreach(var member in existTask.Member.ToList())
//                        {
//                            if (existMember.MemberId == member.MemberId)
//                            {
//                                existTask.Done = true;
//                                existMember.TotalPoint += existTask.Point;
//                                var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == existTask.SprintId);
//                                db.SaveChanges();
//                                return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId });
//                            }
//                        }
                        

//                        return Content("Nothing changed, errors occured.");
//                    }
//                }
//            }
//            return RedirectToAction("Login", "User");
//        }

//        public ActionResult DeleteEpic(int id)
//        {
//            var existTask = new Epic();
//            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
//            db.Epics.Remove(existTask);
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
//        }

//        public ActionResult EditEpic(int id)
//        {
//            Epic existTask = new Epic();
//            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);

//            return View(existTask);
//        }

//        [HttpPost]
//        public ActionResult EditEpic(Epic existTask)
//        {
//            var _existTask = new Epic();
//            _existTask = db.Epics.FirstOrDefault(x => x.EpicId == existTask.EpicId);
//            _existTask.Name = existTask.Name;
//            _existTask.Priority = existTask.Priority;
//            db.SaveChanges();

//            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
//        }

//        public ActionResult AddEpicToSprint(int id)
//        {
//            var existPTask = new Epic();
//            int sprintId = 0;
//            existPTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
//            sprintId = SprintRepository.GetActiveSprint(existPTask.ProjectId);
//            existPTask.SprintId = sprintId;
//            var currentSprint = new Sprint();
//            currentSprint = db.Sprints.FirstOrDefault(x => x.SprintId == sprintId);
//            currentSprint.Epics.Add(existPTask);
//            existPTask.Sprint = currentSprint;
//            db.SaveChanges();
//            return RedirectToAction("IndexEpic", new { id = existPTask.ProjectId });
//        }

        
//    }
//}