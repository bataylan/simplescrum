using ScrumApplication.Entity.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumApplication.Web.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "User mail")]
        public string Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        public bool IsValid(string _usermail, string _password)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existUser = db.Users.FirstOrDefault(x => x.Mail == _usermail);
                if(existUser != null)
                {
                    if (existUser.Password == _password)
                        return true;
                }
            }

            return false;
        }

    }
}