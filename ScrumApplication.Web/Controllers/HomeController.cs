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
            if(UserRepository.IsUserSigned())
            {
                int userId = 0;
                userId = UserRepository.GetUserId();
                var userProjects = ProjectRepository.GetUserProjects(userId);

                return View(userProjects);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
            
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