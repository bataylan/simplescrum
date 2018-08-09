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
                        newMember.TeamId = teamId;
                        newMember.UserId = userId;
                        newMember.Name = existUser.Name;
                        newMember.Mail = existUser.Mail;
                        newMember.RoleCode = roleCode;
                        existTeam.Members.Add(newMember);
                        db.SaveChanges();
                        if (roleCode == 2)
                        {
                            var newManager = new Manager();
                            newManager.UserId = existUser.UserId;
                            newManager.MemberId = newMember.MemberId;
                            newManager.Name = existUser.Name;
                            newManager.Mail = existUser.Mail;
                            db.Managers.Add(newManager);
                            db.SaveChanges();
                            existTeam.ManagerId = newManager.ManagerId;
                            db.SaveChanges();
                        }
                        return true;
                    }

                }

            }

            return false;
        }

        //public static bool IsSprintManager(int sprintId)
        //{
        //    if(UserRepository.IsUserSigned())
        //    {
        //        var existUser = new User();
        //        var existSprint = new Sprint();
        //        var existProject = new Project();
        //        var existTeam = new Team();
        //        var existManager = new Manager();
        //        using (var db = new ScrumApplicationDbContext())
        //        {
        //            existUser = UserRepository.GetUser();
        //            existSprint = db.Sprints.FirstOrDefault(x => x.SprintId == sprintId);
        //            existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existSprint.ProjectId);
        //            existTeam = db.Teams.FirstOrDefault(x => x.TeamId == existProject.TeamId);
        //            existManager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
        //            if (existUser.UserId == existManager.UserId)
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        public static bool IsProjectManager(int projectId)
        {
            if(UserRepository.IsUserSigned())
            {
                var existUser = new User();
                var existProject = new Project();
                var existTeam = new Team();
                var existManager = new Manager();
                using (var db = new ScrumApplicationDbContext())
                {
                    existUser = UserRepository.GetUser();
                    existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                    existTeam = db.Teams.FirstOrDefault(x => x.TeamId == existProject.TeamId);
                    existManager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
                    if (existUser.UserId == existManager.UserId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsTeamManager(int teamId)
        {
            if(UserRepository.IsUserSigned())
            {
                var existUser = new User();
                var existTeam = new Team();
                var existManager = new Manager();
                using (var db = new ScrumApplicationDbContext())
                {
                    existUser = UserRepository.GetUser();
                    existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                    existManager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
                    if (existUser.UserId == existManager.UserId)
                    {
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
                }
                
            }

            return _teamList;
        }

        public static bool ChangeManager(int memberId, int teamId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == teamId);
                //member that will be new manager
                var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId );
                //manager that will be deleted
                var existManager = db.Managers.FirstOrDefault(x => x.ManagerId == existTeam.ManagerId);
                //member that role code will change to 3
                var _existMemberManager = db.Members.FirstOrDefault(x => x.MemberId == existManager.MemberId);
                if(existTeam != null &&  existMember != null && existTeam.TeamId == existMember.TeamId &&
                    existManager != null && _existMemberManager != null)
                {
                    //exist manager removed because every team can have one manager 
                    //and manager have one team
                    db.Managers.Remove(existManager);
                    //member that will be manager role code updated
                    existMember.RoleCode = 2;
                    //member that was manager role code updated
                    _existMemberManager.RoleCode = 3;
                    var newManager = new Manager();
                    newManager.UserId = existMember.UserId;
                    newManager.MemberId = existMember.MemberId;
                    newManager.Mail = existMember.Mail;
                    newManager.Name = existMember.Name;
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

        public static int GetUserMemberIdFromProjectId(int projectId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existProject = new Project();
                var existTeam = new Team();
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                existTeam = db.Teams.FirstOrDefault(x => x.TeamId == existProject.TeamId);
                int userId = UserRepository.GetUserId();
                foreach(var member in db.Members)
                {
                    if(member.TeamId == existTeam.TeamId && member.UserId == userId)
                    {
                        return member.MemberId;
                    }
                }
                return 0;
            }
        }

        public static int GetTeamIdFromProductBacklog(int taskId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existBacklog = new ProductBacklog();
                var existProject = new Project();
                var existTeam = new Team();
                existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == taskId);
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existBacklog.ProjectId);
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
