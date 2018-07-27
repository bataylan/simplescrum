using ScrumApplication.DAL.Repositories;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using ScrumApplication.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ScrumApplication.Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            var user = new User();
            return View(user);

          

        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existUser = db.Users.FirstOrDefault(x => x.Mail.Equals(user.Mail));
                if (existUser != null && existUser.Password == user.Password)
                {
                    FormsAuthentication.SetAuthCookie(existUser.Mail, true);
                    UserRepository.UpdateUserCookie(existUser.UserId);

                    return RedirectToAction("Index", "Home",new { id = UserRepository.GetUserId() });
                    
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
                
            
            return View(user);
        }

        public ActionResult Register()
        {
            var newUser = new User();
            return View(newUser);
        }

        [HttpPost]
        public ActionResult Register(User newUser)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                if(db.Users.FirstOrDefault(x=>x.Mail == newUser.Mail) == null)
                {
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    
                    FormsAuthentication.SetAuthCookie(newUser.Mail, true);
                    UserRepository.UpdateUserCookie(newUser.UserId);

                    return RedirectToAction("Index", "Team", new { id = UserRepository.GetUserId() });
                }

            }
                return View(newUser);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            if (UserRepository.IsUserSigned())
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get("UserId");
                cookie.Expires = DateTime.Now.AddDays(-1d);

            }

            return RedirectToAction("Login", "User");
        }
    }
}