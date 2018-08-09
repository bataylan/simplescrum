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
    public class TeamController : Controller
    {
        ScrumApplicationDbContext db;

        public TeamController()
        {
            db = new ScrumApplicationDbContext();
        }
        // GET: Team
        public ActionResult Index()
        {
            var usersTeams = new List<Team>();
            usersTeams = TeamRepository.GetTeams(UserRepository.GetUserId());

            return View(usersTeams);
        }

        public ActionResult Select(int id)
        {
            return RedirectToAction("Index", "Project",new {id = id });
        }
               
        public ActionResult Create()
        {
            var newTeam = new Team();
                        
            return View(newTeam);
        }

        [HttpPost]
        public ActionResult Create(Team newTeam)
        {
            if (UserRepository.IsUserSigned())
            {
                int userId = UserRepository.GetUserId();

                db.Teams.Add(newTeam);
                db.SaveChanges();
                TeamRepository.AddUserToTeam(userId, newTeam.TeamId, 2);

                return RedirectToAction("Index", "Team", new { id = userId });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult BackToList ()
        {
            if (UserRepository.IsUserSigned())
            {
                int userId = UserRepository.GetUserId();
                return RedirectToAction("Index", "Team", new { id = userId });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult ChangeManager(int id)
        {
            var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == id);
            var manager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);

            return View(manager);
        }

        [HttpPost]
        public ActionResult ChangeManager(Manager manager)
        {
            var existMember = new Member();
            var existTeam = db.Teams.FirstOrDefault(x => x.ManagerId == manager.ManagerId);
            existMember = db.Members.FirstOrDefault(x => x.Mail == manager.Mail && x.TeamId == existTeam.TeamId);
            TeamRepository.ChangeManager(existMember.MemberId, existTeam.TeamId);

            return RedirectToAction("Edit", "Team", new { id = existTeam.TeamId });
        }

        public ActionResult Edit(int id)
        {
            var existTeam = new Team();
            existTeam = db.Teams.FirstOrDefault(x => x.TeamId == id);

            return View(existTeam);
        }

        [HttpPost]
        public ActionResult Edit(Team _existTeam)
        {
            if (UserRepository.IsUserSigned())
            {
                int userId = UserRepository.GetUserId();
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == _existTeam.TeamId);
                existTeam.Name = _existTeam.Name;
                db.SaveChanges();

                return RedirectToAction("Index", "Team", new { id = userId });
            }
            
            return RedirectToAction("Login", "User");
        }

        //Sadece Manager'a bu butonu göster, Manager olup olmadığını kontrol et.
        //test et
        public ActionResult Delete(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == id);
                var teamManager = db.Managers.FirstOrDefault(x=>x.ManagerId == existTeam.ManagerId);
                if (UserRepository.GetUserId() == teamManager.UserId)
                {
                    if (TeamRepository.DeleteTeam(id))
                    {
                        int userId = UserRepository.GetUserId();

                        return RedirectToAction("Index", "Team", new { id = userId });
                    }
                    return Content("Cannot delete, Team has active projects.");
                }

            }
            
            return RedirectToAction("Login", "User");
        }

        public  ActionResult AddUser(int id)
        {
            var newMember = new Member();
            newMember.TeamId = id;

            return View(newMember);
        }
        [HttpPost]
        public ActionResult AddUser(Member newMember)
        {
            if (UserRepository.IsUserSigned())
            {
                var existUser = db.Users.FirstOrDefault(x => x.Mail == newMember.Mail);
                TeamRepository.AddUserToTeam(existUser.UserId, newMember.TeamId, 3);

                return RedirectToAction("Edit", "Team", new { id = newMember.TeamId });
            }

            return RedirectToAction("Login", "User");
        }

        //Takımdan çıkmak için edit'e quit butonu eklenecek, cookie'den user id ve o anki takım id'si çekilecek.

        public ActionResult RemoveMemberFromTeam(int id)
        {
            //Takımdan kullanıcı çıkarmak için kullanıcının ve takımın id'si alınacak

            if (UserRepository.IsUserSigned())
            {
                var existMember = db.Members.FirstOrDefault(x => x.MemberId == id);
                var isDone = TeamRepository.RemoveMemberFromTeam(id, existMember.TeamId);
                if(isDone)
                {
                    return RedirectToAction("Edit", "Team" /*buraya takımın id'si eklenmeli*/);
                }
                //return'e alacağı id yazılmalı
                return Content("User can't deleted.");
            }

            return RedirectToAction("Login", "User");
        }
    }
}