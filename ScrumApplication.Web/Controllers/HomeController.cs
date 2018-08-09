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
    public class HomeController : Controller
    {
        ScrumApplicationDbContext db;
        public HomeController()
        {
            db = new ScrumApplicationDbContext();
        }

        public ActionResult Index()
        {
            int userId = 0;
            userId = UserRepository.GetUserId();
            var userProjects = ProjectRepository.GetUserProjects(userId);
            //var homeVM = ProjectRepository.PrepareHomeViewModel(userId);
            //var userTeams = new List<Team>();
            //userTeams = TeamRepository.GetTeams(userId);
            //var teamProjects = new List<Project>();
            //var userActiveProjects = new List<Project>();
            //foreach(var team in userTeams)
            //{
            //    teamProjects = ProjectRepository.GetProjects(team.TeamId);
            //    foreach (var project in teamProjects)
            //    {
            //        if(!project.IsDone)
            //        {
            //            userActiveProjects.Add(project);
            //        }
            //    }
            //}
            //userActiveProjects = userActiveProjects.OrderBy(x => x.CreatedDate).ToList();
            
            return View(userProjects);
        }

        public ActionResult GoSprint(int id)
        {

            return RedirectToAction("Index","Sprint", new { id = id });
        }
        public ActionResult Teams()
        {
            if (UserRepository.IsUserSigned())
            {
                int userId = UserRepository.GetUserId();
                return RedirectToAction("Index", "Team", new { id = userId });
            }

            return RedirectToAction("Login", "User");
        }
        
    }
}