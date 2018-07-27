using ScrumApplication.DAL.Repositories;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using ScrumApplication.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumApplication.Web.Controllers
{
    public class SprintController : Controller
    {
        ScrumApplicationDbContext db;

        public SprintController()
        {
            db = new ScrumApplicationDbContext();
        }
        // GET: Sprint
        public ActionResult Index(int id)
        {
            var newModel = SprintRepository.GetSprintModel(id);

            return View(newModel);
        }

        public ActionResult EndSprint(int id)
        {
            var existSprint = new Sprint();
            var newSprint = new Sprint();
            existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == id);
            existSprint.IsDone = true;
            var uncompletedTasks = new List<SprintTask>();
            uncompletedTasks = existSprint.SprintTasks.ToList();
            var uncompletedPTasks = new List<ProjectTask>();
            uncompletedPTasks = existSprint.ProjectTasks.ToList();
            newSprint.SprintId = SprintRepository.CreateSprint(existSprint.ProjectId);
            newSprint = db.Sprints.FirstOrDefault(x => x.SprintId == newSprint.SprintId);

            if (uncompletedTasks != null)
            {
                var query = from task in uncompletedTasks
                            where task.Done == false
                            orderby task.Priority
                            select task;

                uncompletedTasks = query.ToList();

                foreach (var task in uncompletedTasks)
                {
                    task.Sprint = newSprint;
                    task.SprintId = newSprint.SprintId;
                    newSprint.SprintTasks.Add(task);
                }
            }
            else
            {
                foreach(var ptask in uncompletedPTasks)
                {
                    ptask.IsDone = true;
                }
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Sprint", new { id = existSprint.ProjectId });
        }

        public ActionResult IndexEpic(int id)
        {
            List<ProjectTask> projectTasks = new List<ProjectTask>();
            projectTasks = ProjectTaskRepository.GetProjectTasks(id);
            if(projectTasks.Count == 0)
            {
                var pTask = new ProjectTask();
                pTask.ProjectId = id;
                projectTasks.Add(pTask);
            }
            return View(projectTasks);
        }

        //public ActionResult Edit(int id)
        //{
        //    var existSPTask = new SprintTask();
        //    existSPTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == id);

        //    return View(existSPTask);
        //}

        //[HttpPost]
        //public ActionResult Edit(SprintTask existSPTask)
        //{
        //    var _existSPTask = new SprintTask();
        //    _existSPTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == existSPTask.SprintTaskId);
        //    _existSPTask.Name = existSPTask.Name;
        //    _existSPTask.Priority = existSPTask.Priority;
        //    _existSPTask.Point = existSPTask.Point;
        //    _existSPTask.Done = existSPTask.Done;
        //    if(existSPTask.Done == false && _existSPTask.Done == true && existSPTask.UserId != null)
        //    {
        //        //Puanı ekle
        //    }
                
        //    else if(existSPTask.Done == true && _existSPTask.Done == false && existSPTask.UserId != null)
        //    {
        //        //Puanı düşür
        //    }
        //    //Eğer Taskın ilk hali done değilse ve son hali done'sa, kullanıcıya puan eklenecek
        //    db.SaveChanges();
        //    return RedirectToAction("Index", new { id = existSPTask.Sprint.ProjectId });
        //}

        public ActionResult CreateTask(int id)
        {
            var newTask = new SprintTask();
            newTask.SprintId = id;
            var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == id);
            newTask.ProjectId = existSprint.ProjectId;
            newTask.Sprint = existSprint;

            return View(newTask);
        }

        [HttpPost]
        public ActionResult CreateTask(SprintTask newTask)
        {
            var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == newTask.SprintId);
            db.SprintTasks.Add(newTask);
            //existSprint.SprintTasks.Add(newTask);
            db.SaveChanges();

            return RedirectToAction("Index", "Sprint", new { id = existSprint.ProjectId});
        }

        public ActionResult EditSprint(int id)
        {
            var existSprint = new Sprint();
            existSprint = db.Sprints.FirstOrDefault(x=>x.SprintId == id);
            return View(existSprint);
        }

        [HttpPost]
        public ActionResult EditSprint(Sprint existSprint)
        {
            var _existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == existSprint.SprintId);
            _existSprint.Name = existSprint.Name;
            _existSprint.DefaultTime = existSprint.DefaultTime;
            db.SaveChanges();
            return View();
        }

        public ActionResult TakeIt(int id)
        {
            var existTask = new SprintTask();
            existTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == id);
            if (UserRepository.IsUserSigned())
            {
                int teamId = TeamRepository.GetTeamIdFromSprintTask(id);
                if(teamId != 0)
                {
                    var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                    int userId = UserRepository.GetUserId();
                    var existMember = existTeam.Members.ToList().FirstOrDefault(x => x.UserId == userId);
                    if( existMember != null)
                    {
                        existTask.MemberId = existMember.MemberId;
                        existTask.Member = existMember;
                        existTask.AssignedTo = existMember.Name;
                        db.SaveChanges();

                        return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId });
                    }
                }
                
            }

            return RedirectToAction("Login", "User");


        }

        public ActionResult GiveIn(int id)
        {
            var existTask = new SprintTask();
            existTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == id);
            if (UserRepository.IsUserSigned())
            {
                existTask.MemberId = null;
                db.SaveChanges();
            }


            return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId});

        }

        public ActionResult BackToList(int id)
        {
            if (UserRepository.IsUserSigned())
            {

                return RedirectToAction("Index", "Sprint", new { id = id });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult CompleteEpic(int id)
        {
            var existTask = new ProjectTask();
            existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);
            existTask.IsDone = true;
            db.SaveChanges();

            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
        }

        public ActionResult GreenEpic(int id)
        {
            var existTask = new ProjectTask();
            existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);
            existTask.IsDone = false;
            db.SaveChanges();

            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId });
        }

        public ActionResult CreateEpic(int id)
        {
            var newProjectTask = new ProjectTask();
            newProjectTask.ProjectId = id;

            return View(newProjectTask);
        }

        [HttpPost]
        public ActionResult CreateEpic(ProjectTask newProjectTask)
        {
            db.ProjectTasks.Add(newProjectTask);
            db.SaveChanges();

            return RedirectToAction("IndexEpic", "Sprint", new { id = newProjectTask.ProjectId});
        }

        public ActionResult Complete(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                var existTask = new SprintTask();
                int userId = UserRepository.GetUserId();
                existTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == id);
                int teamId = TeamRepository.GetTeamIdFromSprintTask(id);
                if(teamId != 0)
                {
                    var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                    var existMember = existTeam.Members.ToList().FirstOrDefault(x => x.UserId == userId);
                    if (existMember != null)
                    {
                        
                        if(existMember.MemberId == existTask.MemberId)
                        {
                            existTask.Done = true;
                            existMember.TotalPoint += existTask.Point;
                            var existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == existTask.SprintId);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Sprint", new { id = existTask.Sprint.ProjectId });
                        }

                        return Content("Nothing changed, errors occured.");
                    }
                }
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult DeleteEpic(int id)
        {
            var existTask = new ProjectTask();
            existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);
            db.ProjectTasks.Remove(existTask);
            db.SaveChanges();

            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
        }

        public ActionResult EditEpic(int id)
        {
            ProjectTask existTask = new ProjectTask();
            existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);

            return View(existTask);
        }

        [HttpPost]
        public ActionResult EditEpic(ProjectTask existTask)
        {
            var _existTask = new ProjectTask();
            _existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == existTask.ProjectTaskId);
            _existTask.Name = existTask.Name;
            _existTask.Priority = existTask.Priority;
            db.SaveChanges();

            return RedirectToAction("IndexEpic", "Sprint", new { id = existTask.ProjectId});
        }

        public ActionResult AddEpicToSprint(int id)
        {
            var existPTask = new ProjectTask();
            int sprintId = 0;
            existPTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);
            sprintId = SprintRepository.GetActiveSprint(existPTask.ProjectId);
            existPTask.SprintId = sprintId;
            var currentSprint = new Sprint();
            currentSprint = db.Sprints.FirstOrDefault(x => x.SprintId == sprintId);
            currentSprint.ProjectTasks.Add(existPTask);
            existPTask.Sprint = currentSprint;
            db.SaveChanges();
            return RedirectToAction("IndexEpic", new { id = existPTask.ProjectId });
        }

        
    }
}