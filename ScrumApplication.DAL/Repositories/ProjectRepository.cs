using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;

namespace ScrumApplication.DAL.Repositories
{
    public static class ProjectRepository
    {
        public static void AddProject(string name)
        {
            var _newProject = new Project();
            _newProject.Name = name;

            using (var db = new ScrumApplicationDbContext())
            {
                db.Projects.Add(_newProject);
                db.SaveChanges();
                ActivityRepository.ActivityCreator
                    ("created" + _newProject.Name ,_newProject.ProjectId, null);
            }
        }

        public static List<Project> GetProjects(int teamId)
        {
            var _projectList = new List<Project>();

            using (var db = new ScrumApplicationDbContext())
            {
                var query = from project in db.Projects
                            where project.TeamId == teamId && project.DayCount > 0
                            orderby project.Name ascending
                            select project;
                _projectList = query.ToList();
            }

            return _projectList;
        }

        public static List<Project> GetUserProjects(int userId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var userTeams = TeamRepository.GetTeams(userId);
                var userProjects = new List<Project>();
                foreach(var team in userTeams)
                {
                    foreach(var project in db.Projects)
                    {
                        if(project.TeamId == team.TeamId && project.IsDone == false)
                        {
                            userProjects.Add(project);
                        }
                    }
                }
                return userProjects;
            }
        }

        //public static SprintModel GetSprintModel(int projectId, int sprintNo)
        //{
        //    using (var db = new ScrumApplicationDbContext())
        //    {
        //        var existProject = new Project();
        //        existProject= db.Projects.FirstOrDefault(x=>x.ProjectId == projectId);
        //        var newModel = new SprintModel();
        //        newModel.ProjectId = projectId;
        //        var sprintBacklogs = new List<ProductBacklog>();
        //        var projectEpics = new List<Epic>();
        //        sprintBacklogs = existProject.ProductBacklogs.ToList();
        //        projectEpics = existProject.Epics.ToList();
        //        if (sprintBacklogs != null)
        //        {
        //            var query = from backlog in sprintBacklogs
        //                        where backlog.SprintNo == sprintNo
        //                        orderby backlog.EpicId ascending, backlog.Priority ascending
        //                        select backlog;
        //            newModel.ProductBacklogs = query.ToList();
        //        }
        //        if (projectEpics != null)
        //        {
        //            var equery = from epic in projectEpics
        //                         orderby epic.IsDone ascending, epic.Priority ascending
        //                         select epic;
        //            newModel.Epics = equery.ToList();
        //        }
        //        newModel.ProjectId = existProject.ProjectId;
        //        newModel.SprintNo = sprintNo;

        //        return newModel;
        //    }

        //}

        public static int GetActiveSprintNo(int projectId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existProject = new Project();
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                

                return existProject.CurrentSprintNo;
            }

        }

        public static List<Epic> GetProjectEpics (int projectId)
        {
            var projectEpics = new List<Epic>();

            using (var db = new ScrumApplicationDbContext())
            {
                var query = from epic in db.Epics
                            where epic.ProjectId == projectId
                            orderby epic.IsDone ascending, epic.Priority ascending
                            select epic;
                projectEpics = query.ToList();
            }

            return projectEpics;
        }

        public static int GetSprintCount (int projectId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                int sprintCount = 0;
                var query = from backlog in db.ProductBacklogs
                            where backlog.ProjectId == projectId
                            select backlog;
                foreach(var backlog in query.ToList())
                {
                    if(backlog.SprintNo > sprintCount)
                    {
                        sprintCount = backlog.SprintNo;
                    }
                }

                return sprintCount;
            }
            
        }

        public static List<ProductBacklog> GetProjectBacklogs(int projectId, int sortBy = 0)
        {
            var projectBacklogs = new List<ProductBacklog>();
            using (var db = new ScrumApplicationDbContext())
            {
                var query = from backlog in db.ProductBacklogs
                            where backlog.ProjectId == projectId
                            select backlog;
                projectBacklogs = query.ToList().SortBacklogs(sortBy);
            }
            return projectBacklogs;
        }

        public static List<ProductBacklog> GetProjectBacklogs(int projectId, int sprintNo, int sortBy = 0)
        {
            var projectBacklogs = new List<ProductBacklog>();
            using (var db = new ScrumApplicationDbContext())
            {
                if(sprintNo == 0)
                {
                    var query = from backlog in db.ProductBacklogs
                                where backlog.ProjectId == projectId
                                select backlog;
                    projectBacklogs = query.ToList().SortBacklogs(sortBy);
                }
                else
                {
                    var query = from backlog in db.ProductBacklogs
                                where backlog.ProjectId == projectId && backlog.SprintNo == sprintNo
                                select backlog;
                    projectBacklogs = query.ToList().SortBacklogs(sortBy);
                }
            }
            return projectBacklogs;
        }
        public static List<ProductBacklog> GetSprintBacklogs(int projectId, int sprintNo)
        {
            var sprintBacklogs = new List<ProductBacklog>();
            using (var db = new ScrumApplicationDbContext())
            {
                var query = from backlog in db.ProductBacklogs
                            where backlog.ProjectId == projectId && backlog.SprintNo == sprintNo
                            orderby backlog.Priority ascending, backlog.Done ascending
                            select backlog;
                sprintBacklogs = query.ToList();
            }
            return sprintBacklogs;
        }
        public static List<ProductBacklog> SortBacklogs(this List<ProductBacklog> list, int sortBy)
        {
            var existList = new List<ProductBacklog>();

            if(sortBy == (int)BacklogSort.PriorityAsc)
            {
                existList = list.OrderBy(x => x.Done).ThenBy(x=>x.Priority).ToList();
            }
            else if(sortBy == (int)BacklogSort.PriorityDesc)
            {
                existList = list.OrderBy(x => x.Done).ThenByDescending(x => x.Priority).ToList();
            }
            else if(sortBy == (int)BacklogSort.SprintAsc)
            {
                existList = list.OrderBy(x => x.Done).ThenBy(x => x.SprintNo).ToList();
            }
            else if(sortBy == (int)BacklogSort.SprintDesc)
            {
                existList = list.OrderBy(x => x.Done).ThenByDescending(x => x.SprintNo).ToList();
            }
            else if(sortBy == (int)BacklogSort.StoryPointAsc)
            {
                existList = list.OrderBy(x => x.Done).ThenBy(x => x.StoryPoint).ToList();
            }
            else
            {
                existList = list.OrderBy(x => x.Done).ThenByDescending(x => x.StoryPoint).ToList();
            }
            return existList;
        }

        

        public static BacklogViewModel GetBacklogViewModel(int productBacklogId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var backlogModel = new BacklogViewModel();
                backlogModel.Backlog = db.ProductBacklogs.FirstOrDefault
                    (x => x.ProductBacklogId == productBacklogId);
                var existProject = new Project();
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == backlogModel.Backlog.ProjectId);
                backlogModel.ProjectEpics = EpicRepository.GetEpics(backlogModel.Backlog.ProjectId);
                backlogModel.Epic.ProjectId = backlogModel.Backlog.ProjectId;
                var query = from btm in db.BacklogToMembers
                            where btm.ProductBacklogId == backlogModel.Backlog.ProductBacklogId
                            select btm;
                var teamMembers = new List<Member>();
                var tquery = from member in db.Members
                             where member.TeamId == existProject.TeamId
                             select member;
                teamMembers = tquery.ToList();
                var unAssignedTeamMembers = new List<Member>();
                unAssignedTeamMembers = teamMembers.ToList();
                unAssignedTeamMembers = unAssignedTeamMembers.OrderBy(x => x.MemberId).ToList();
                foreach (var btm in query.ToList())
                {
                    var member = new MemberViewModel();
                    member.MemberId = btm.MemberId;
                    member.Name = btm.MemberName;
                    backlogModel.AssignedMembers.Add(member);
                    foreach (var tmember in teamMembers)
                    {
                        if (tmember.MemberId == member.MemberId)
                        {
                            unAssignedTeamMembers.Remove(tmember);
                        }
                    }
                }
                foreach (var umember in unAssignedTeamMembers)
                {
                    var memberModel = new MemberViewModel();
                    memberModel.MemberId = umember.MemberId;
                    memberModel.Name = umember.Name;
                    backlogModel.UnAssignedMembers.Add(memberModel);
                }
                var cquery = from comment in db.Comments
                            where comment.ProductBacklogId == backlogModel.Backlog.ProductBacklogId
                            orderby comment.CommentId descending
                            select comment;

                backlogModel.BacklogComments = cquery.ToList();
                return backlogModel;
            }
        }

        public static bool UnAssignUser(int backlogId, int memberId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                if(IsUserAssigned(backlogId, memberId))
                {
                    var existBTM = db.BacklogToMembers.FirstOrDefault(x => x.MemberId == memberId && x.ProductBacklogId == backlogId);
                    if(existBTM != null)
                    {
                        db.BacklogToMembers.Remove(existBTM);
                        db.SaveChanges();
                        var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
                        ActivityRepository.ActivityCreator
                    ("unassigned " + existBTM.MemberName + " from " + existBacklog.Name, 
                    existBacklog.ProjectId, existBacklog.ProductBacklogId);
                        return true;
                    }
                }
                return false;
            }
        }
        //Check is user assigned to selected backlog, return bool 
        public static bool IsUserAssigned(int backlogId, int? memberId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
                if(!memberId.HasValue)
                {
                    memberId = TeamRepository.GetUserMemberIdFromProjectId(existBacklog.ProjectId);
                }
                if(memberId != 0 && existBacklog != null)
                {
                    var existBTM = db.BacklogToMembers.FirstOrDefault(x => x.MemberId == memberId && x.ProductBacklogId == backlogId);
                    if(existBTM != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static bool CompleteBacklog(int backlogId, int? memberId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existTask = new ProductBacklog();
                existTask = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
                if(!memberId.HasValue)
                {
                    memberId = TeamRepository.GetUserMemberIdFromProjectId(existTask.ProjectId);
                }
                var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                if (existMember != null)
                {
                    if (ProjectRepository.IsUserAssigned(backlogId, existMember.MemberId))
                    {
                        if(existTask.BacklogStatus < Status.Completed)
                        {
                            existTask.BacklogStatus = Status.Completed;
                        }
                        existTask.Done = true;
                        existMember.TotalPoint += existTask.StoryPoint;
                        db.SaveChanges();
                        ActivityRepository.ActivityCreator
                    ("completed " + existTask.Name, existTask.ProjectId, existTask.ProductBacklogId);
                        return true;
                    }

                }
                return false;
            }
                
        }

        public static HomeViewModel PrepareHomeViewModel(int userId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existUser = db.Users.FirstOrDefault(x => x.UserId == userId);
                var userTeams = TeamRepository.GetTeams(userId);
                var homeVM = new HomeViewModel
                {
                    UserId = userId,
                    UserName = existUser.Name,
                    UserTeams = userTeams,
                    UserActiveProjects = new List<HomeProjectViewModel>()
                };
                foreach (var team in userTeams)
                {
                    var teamProjects = GetProjects(team.TeamId);
                    foreach (var project in teamProjects)
                    {
                        if(!project.IsDone)
                        {
                            var memberId = TeamRepository.GetUserMemberIdFromProjectId(project.ProjectId);
                            var member = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                            var homeProjectVM = new HomeProjectViewModel
                            {
                                ProjectId = project.ProjectId,
                                ProjectName = project.Name,
                                TeamId = project.TeamId ?? default(int),
                                TeamName = team.Name,
                                MemberId = memberId,
                                RoleCode = member.RoleCode,
                                AssignedActiveBacklogs = new List<ProductBacklog>()
                            };
                            var projectBacklogs = GetProjectBacklogs(project.ProjectId);
                            foreach(var backlog in projectBacklogs)
                            {
                                if (!backlog.Done && IsUserAssigned(backlog.ProductBacklogId, memberId) && backlog.SprintNo==project.CurrentSprintNo)
                                {
                                    var btm = db.BacklogToMembers.FirstOrDefault
                                        (x => x.MemberId == memberId && x.ProductBacklogId == backlog.ProductBacklogId);
                                    if(btm != null)
                                    {
                                        homeProjectVM.AssignedActiveBacklogs.Add(backlog);
                                    }
                                }
                            }
                            if(homeProjectVM != null)
                            {
                                homeProjectVM.AssignedActiveBacklogs = homeProjectVM.AssignedActiveBacklogs.OrderBy(x => x.Priority).ToList();
                                homeVM.UserActiveProjects.Add(homeProjectVM);
                            }
                        }
                    }
                }
                return homeVM;
            }
                

        }

        public static int GetProjectSprintCount(int projectId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                var projectBacklogs = ProjectRepository.GetProjectBacklogs(projectId);
                int sprintCount = 0;
                foreach(var backlog in projectBacklogs)
                {
                    if(sprintCount < backlog.SprintNo)
                    {
                        sprintCount = backlog.SprintNo;
                    }
                }
                return sprintCount;
            }
        }

        public static ProjectViewModel PrepareProjectHome(int projectId)
        {
            using (var db = new ScrumApplicationDbContext())
            {
                var projectModel = new ProjectViewModel();
                projectModel.Project = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                if (projectModel.Project != null)
                {
                    var tquery = from member in db.Members
                                 where member.TeamId == projectModel.Project.TeamId
                                 select member;
                    projectModel.ProjectTeamMembers = tquery.ToList();

                    var vquery = from activity in db.Activities
                                 where activity.ProjectId == projectId
                                 orderby activity.CreatedTime ascending
                                 select activity;
                    projectModel.ProjectActivities = vquery.ToList();

                    return projectModel;
                }
                
            }
            return null;
        }
    }
}
