using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ScrumApplication.DAL.Repositories
{
    public class TeamRepository
    {
        public static void AddTeam(string name)
        {
            var _newTeam = new Team();
            _newTeam.Name = name;

            using (var db = new ScrumApplicationDbContext())
            {
                db.Teams.Add(_newTeam);
                db.SaveChanges();
            }
        }

        public static void AddProjectToTeam(int projectId, int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                existTeam.Projects.Add(existProject);
                db.SaveChanges();
            }
        }

        //member'ı tamamen sil
        public static bool RemoveMemberFromTeam(int memberId, int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                existTeam.Members.Remove(existMember);
                db.Members.Remove(existMember);
                db.SaveChanges();
            }
                
            return false;
        }

        public static bool AddUserToTeam(int userId, int teamId, int roleCode)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTeam = new Team();
                existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                bool isMemberAdded = false;
                if (existTeam != null)
                {
                    foreach (var member in existTeam.Members.ToList())
                    {
                        if (member.UserId == userId)
                        {
                            isMemberAdded = true;
                            break;
                        }
                    }
                    if (!isMemberAdded)
                    {
                        var newMember = new Member();
                        var existUser = db.Users.FirstOrDefault(x => x.UserId == userId);
                        if (roleCode < 3)
                        {
                            var newManager = new Manager();
                            newManager.UserId = existUser.UserId;
                            newManager.User = existUser;
                            newManager.Name = existUser.Name;
                            newManager.Mail = existUser.Mail;
                            db.Managers.Add(newManager);
                            db.SaveChanges();
                            existTeam.ManagerId = newManager.ManagerId;
                            existTeam.Manager = newManager;
                            db.SaveChanges();
                        }
                        else
                        {
                            newMember.TeamId = teamId;
                            newMember.Team = existTeam;
                            newMember.UserId = userId;
                            newMember.Name = existUser.Name;
                            newMember.Mail = existUser.Mail;
                            newMember.RoleCode = roleCode;
                            existTeam.Members.Add(newMember);
                            db.SaveChanges();
                        }
                        return true;
                    }

                }

            }

            return false;
        }

        //test et
        public static bool DeleteTeam(int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTeam = new Team();
                existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                var teamProjects = new List<Project>();
                teamProjects = existTeam.Projects.ToList();
                if (teamProjects.Count != 0)
                {
                    foreach (var p in teamProjects)
                    {
                        if (p.DayCount > 0)
                        {
                            return false;
                        }
                        else
                        {
                            teamProjects.Remove(p);
                        }
                    }
                }
                else if (teamProjects.Count != 0)
                {
                    db.Teams.Remove(existTeam);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public static List<Team> GetTeams(int userId)
        {
            var _teamList = new List<Team>();
            var existUser = new User();
            using (var db = new ScrumApplicationDbContext())
            {
                if (userId != 0)
                {
                    var query = from member in db.Members
                                where member.UserId == userId
                                select member.Team;
                    _teamList = query.ToList();

                    foreach(var manager in db.Managers)
                    {

                        if(manager.UserId == userId)
                        {
                            var existTeam = db.Teams.FirstOrDefault(x => x.ManagerId == manager.ManagerId);
                            _teamList.Add(existTeam);
                        }
                    }
                }
                
            }

            return _teamList;
        }

        public static bool ChangeManager(int userId, int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                var existUser = db.Users.FirstOrDefault(x => x.UserId == userId);
                var manager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
                var managerUser = db.Users.FirstOrDefault(x => x.UserId == manager.UserId);
                if(existTeam != null && existUser != null)
                {
                    if (manager != null)
                    {
                        db.Managers.Remove(manager);
                        TeamRepository.AddUserToTeam(managerUser.UserId, existTeam.TeamId, 3);
                        db.SaveChanges();
                    }
                    foreach(var member in existTeam.Members.ToList())
                    {
                        if(member.UserId == userId)
                        {
                            existTeam.Members.Remove(member);
                            db.Members.Remove(member);
                            db.SaveChanges();
                            break;
                        }
                    }
                    var newManager = new Manager();
                    newManager.UserId = existUser.UserId;
                    newManager.User = existUser;
                    newManager.Mail = existUser.Mail;
                    newManager.Name = existUser.Name;
                    existUser.Managers.Add(newManager);
                    db.Managers.Add(newManager);
                    db.SaveChanges();
                    existTeam.ManagerId = newManager.ManagerId;
                    existTeam.Manager = newManager;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static int GetLastTeamId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("LastTeam");
            return Int32.Parse(cookie.Value);
        }

        public static int GetTeamIdFromSprintTask(int taskId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTask = new SprintTask();
                var existSprint = new Sprint();
                var existProject = new Project();
                var existTeam = new Team();
                existTask = db.SprintTasks.FirstOrDefault(x => x.SprintTaskId == taskId);
                existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == existTask.SprintId);
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existSprint.ProjectId);
                existTeam = db.Teams.FirstOrDefault(x => x.TeamId == existProject.TeamId);

                if(existTeam.TeamId != 0)
                {
                    return existTeam.TeamId;
                }
                return 0;
            }
        }
        
        public static Manager GetTeamManager(int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                
                if(existTeam != null && existTeam.ManagerId != 0)
                {
                    var manager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
                    return manager;
                }
                return null;
            }
        }

        public static bool IsUserManagerOfTeam(int userId, int teamId)
        {
            if(UserRepository.IsUserSigned())
            {
                var manager = GetTeamManager(teamId);
                if (manager != null)
                {
                    if (UserRepository.GetUserId() == manager.UserId)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
    }

}
