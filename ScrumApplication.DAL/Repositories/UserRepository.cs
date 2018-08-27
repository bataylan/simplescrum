using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrumApplication.Entity.Models;
using ScrumApplication.Entity.DbContext;
using System.Web;

namespace ScrumApplication.DAL.Repositories
{
    public class UserRepository
    {
        public static void AddUser(string name, string email, string password)
        {
            var _newUser = new User();
            _newUser.Name = name;
            _newUser.Mail = email;
            _newUser.Password = password;
            using (var db = new ScrumApplicationDbContext())
            {
                db.Users.Add(_newUser);
                db.SaveChanges();
            }
        }

        public static bool IsUserSigned()
        {
            if(GetUserId() != 0)
            {
                return true;
            }
            return false;
        }
        public static User GetUser()
        {
            var user = new User();
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("UserId");
            int userId = Int32.Parse(cookie.Value);
            using (var db = new ScrumApplicationDbContext())
            {
                user = db.Users.FirstOrDefault(x => x.UserId == userId);
                if(user != null)
                {
                    return user;
                }
            }

            return null;
        }

        public static int GetUserId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("UserId");
            if(cookie != null)
            {
                return Int32.Parse(cookie.Value);
            }

            return 0;
        }

        public static void UpdateUserCookie(int id)
        {
            HttpCookie newCookie = new HttpCookie("UserId", id.ToString());
            newCookie.Expires = DateTime.Now.AddDays(1d);
            HttpContext.Current.Response.SetCookie(newCookie);
        }

    }

    //encript ve decript methodları yazılacak

    
}
