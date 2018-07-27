using ScrumApplication.DAL.Repositories;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumApplication.Web.Controllers
{
    public class ProjectController : Controller
    {
        ScrumApplicationDbContext db;
        
        public ProjectController()
        {
            db = new ScrumApplicationDbContext();
        }
        // GET: Project
        public ActionResult Index(int id)
        {
            var teamProjects = new List<Project>();
            teamProjects = ProjectRepository.GetProjects(id);
            if(teamProjects.Count == 0)
            {
                var project = new Project();
                project.TeamId = id;
                teamProjects.Add(project);
            }
            return View(teamProjects);
        }

        public ActionResult Create(int teamId)
        {
            var newProject = new Project();
            newProject.TeamId = teamId;
            return View(newProject);
        }

        [HttpPost]
        public ActionResult Create(Project newProject)
        {
            if (UserRepository.IsUserSigned())
            {
                var existTeam = new Team();
                newProject.EndDate = newProject.CreatedDate.AddDays(newProject.DayCount);
                db.Projects.Add(newProject);
                db.SaveChanges();

                return RedirectToAction("Index", "Project", new { id = newProject.TeamId });
            }

            return RedirectToAction("Login", "User");
        }

        //Done methodunu yaz(projectId alacak)
        public  ActionResult Done()
        {
            return View();
        }

        //RemoveProjectTask methodunu yaz(projectTaskId alacak)

        public ActionResult Edit(int id)
        {
            var existProject = new Project();
            existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);

            return View(existProject);
        }

        [HttpPost]
        public ActionResult Edit(Project existProject)
        {

            if (UserRepository.IsUserSigned())
            {
                var _existProject = new Project();
                _existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existProject.ProjectId);
                _existProject.Name = existProject.Name;
                _existProject.DayCount = existProject.DayCount;
                _existProject.EndDate = existProject.CreatedDate.AddDays(existProject.DayCount);
                _existProject.DefaultSprintTime = existProject.DefaultSprintTime;
                db.SaveChanges();

                return RedirectToAction("Index", "Project", new { id = existProject.TeamId });
            }

            return RedirectToAction("Login", "User");
        }
                
        public ActionResult Delete (int id)
        {
            var existProject = new Project();
            existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (TeamRepository.IsUserManagerOfTeam(UserRepository.GetUserId(), existProject.TeamId ?? default (int)))
            {
                db.Projects.Remove(existProject);
                db.SaveChanges();

                return RedirectToAction("Index", "Project", new { id = existProject.TeamId });
            }    
            
            return RedirectToAction("Login", "User");
            
        }

        public ActionResult DeleteTask(int id)
        {
            var existTask = new ProjectTask();
            existTask = db.ProjectTasks.FirstOrDefault(x => x.ProjectTaskId == id);
            db.ProjectTasks.Remove(existTask);
            db.SaveChanges();

            //HttpCookie cookie = Request.Cookies.Get("LastProject");
            int teamId = TeamRepository.GetLastTeamId();

            return RedirectToAction("Edit", "Project", new { id = teamId });
        }

        public ActionResult Complete(int id)
        {
            var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            existProject.IsDone = true;
            db.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = existProject.TeamId});
        }

        public ActionResult Green(int id)
        {
            var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            existProject.IsDone = false;
            db.SaveChanges();

            return RedirectToAction("Index", "Project", new { id = existProject.TeamId});
        }

        public ActionResult GoSprint(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                return RedirectToAction("Index", "Sprint", new { id = id });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult AllProjectTasks(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                return RedirectToAction("IndexEpic", "Sprint", new { id = id });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult BackToList(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                return RedirectToAction("Index", "Project", new { id = id});
            }

            return RedirectToAction("Login", "User");
        }
        
    }
}