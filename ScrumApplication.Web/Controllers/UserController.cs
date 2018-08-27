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
        ScrumApplicationDbContext db;

        public UserController()
        {
            db = new ScrumApplicationDbContext();
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Account()
        {
            var newModel = new AccountViewModel();
            var user = UserRepository.GetUser();
            newModel.User = user;

            return View(newModel);
        }

        [HttpPost]
        public ActionResult Account(AccountViewModel newModel)
        {
            var existUser = db.Users.FirstOrDefault(x => x.UserId == newModel.User.UserId);
            if(newModel.CurrentPassword != null && newModel.NewPassword != null && newModel.ConfirmNewPassword != null)
            {
                if(newModel.CurrentPassword == existUser.Password)
                {
                    if(newModel.NewPassword == newModel.ConfirmNewPassword)
                    {
                        existUser.Password = newModel.NewPassword;
                        db.SaveChanges();
                    }
                    else
                    {
                        return Content("You entered new password and confirm new password different.");
                    }
                     
                }
                else
                {
                    return Content("You entered wrong current password.");
                }
                
            }
            return RedirectToAction("Account");
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
            var newUser = new UserRegisterViewModel();
            return View(newUser);
        }

        [HttpPost]
        public ActionResult Register(UserRegisterViewModel newUser)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                if(db.Users.FirstOrDefault(x=>x.Mail == newUser.User.Mail) == null)
                {
                    if(newUser.ConfirmPassword == newUser.User.Password)
                    {
                        var _newUser = new User {
                            FirstName = newUser.User.FirstName,
                            LastName = newUser.User.LastName,
                            Name = newUser.User.FirstName + " " + newUser.User.LastName.Substring(0, 1) + ".",
                            Mail = newUser.User.Mail,
                            Password = newUser.User.Password
                        };

                        db.Users.Add(_newUser);
                        db.SaveChanges();

                        FormsAuthentication.SetAuthCookie(newUser.User.Mail, true);
                        UserRepository.UpdateUserCookie(newUser.User.UserId);

                        return RedirectToAction("Index", "Team", new { id = UserRepository.GetUserId() });
                    }
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
                HttpContext.Response.SetCookie(cookie);

            }

            return RedirectToAction("Login", "User");
        }
    }
}