﻿using ScrumApplication.DAL.Repositories;
using ScrumApplication.Entity.DbContext;
using ScrumApplication.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumApplication.Web.Controllers
{
    public class ProjectController : Controller
    {
        ScrumApplicationDbContext db;
        
        public ProjectController()
        {
            db = new ScrumApplicationDbContext();
        }
        // GET: Project
        public ActionResult Index(int id)
        {
            var teamProjects = new List<Project>();
            teamProjects = ProjectRepository.GetProjects(id);
            if(teamProjects.Count == 0)
            {
                var project = new Project();
                project.TeamId = id;
                teamProjects.Add(project);
            }
            return View(teamProjects);
        }

        public ActionResult Create(int? teamId, int? from)
        {
            var existProjectModel = new ProjectEditViewModel();
            if(teamId.HasValue)
            {
                existProjectModel.Project.TeamId = teamId;
            }
            int userId = UserRepository.GetUserId();
            var userCommunityTeams = TeamRepository.GetTeamsForManager(userId);
            existProjectModel.UserTeams = userCommunityTeams;

            return View(existProjectModel);
        }

        [HttpPost]
        public ActionResult Create(ProjectEditViewModel existProjectModel)
        {
            if (UserRepository.IsUserSigned() && TeamRepository.IsTeamManager(existProjectModel.Project.TeamId ?? default(int)))
            {
                //Define project informations
                var newProject = new Project();
                newProject = existProjectModel.Project;
                if (existProjectModel.Project.EndDate <= existProjectModel.Project.CreatedDate)
                {
                    return Content("Please enter an upward EndDate from today's date.");
                }
                db.Projects.Add(newProject);
                db.SaveChanges();

                ActivityRepository.ActivityCreator
                    ("created " + newProject.Name + " project.", newProject.ProjectId, null);

                return RedirectToAction("Index", "Project", new { id = existProjectModel.Project.TeamId });
            }

            return RedirectToAction("Login", "User");

            //if (UserRepository.IsUserSigned())
            //{
            //    var existTeam = new Team();
            //    newProject.EndDate = newProject.CreatedDate.AddDays(newProject.DayCount);
            //    db.Projects.Add(newProject);
            //    db.SaveChanges();
            //    ActivityRepository.ActivityCreator
            //        ("created " + newProject.Name + " project.", newProject.ProjectId, null);
            //    return RedirectToAction("Index", "Project", new { id = newProject.TeamId });
            //}

            //return RedirectToAction("Login", "User");
        }

        //Done methodunu yaz(projectId alacak)
        public  ActionResult Done()
        {
            //ActivityRepository.ActivityCreator
            //        ("completed " + newProject.Name + " project.", newProject.ProjectId, null);
            return View();
        }

        //RemoveEpic methodunu yaz(EpicId alacak)

        public ActionResult Edit(int id, int? from)
        {
            
            var existProjectModel = new ProjectEditViewModel();
            existProjectModel.Project = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (TeamRepository.IsProjectManager(existProjectModel.Project.ProjectId))
            {
                int userId = UserRepository.GetUserId();

                var userCommunityTeams = TeamRepository.GetTeamsForManager(userId);
                existProjectModel.UserTeams = userCommunityTeams;
                existProjectModel.UserTeams = existProjectModel.UserTeams
                        .GroupBy(x => x.TeamId)
                        .Select(x => x.First())
                        .ToList();

                return View(existProjectModel);
            }

            return Content("You are not team manager, you cannot do this operation.");
        }

        [HttpPost]
        public ActionResult Edit(ProjectEditViewModel existProjectModel)
        {

            if (UserRepository.IsUserSigned())
            {
                //Define project informations
                var _existProject = new Project();
                _existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existProjectModel.Project.ProjectId);
                _existProject.TeamId = existProjectModel.Project.TeamId;
                
                if(_existProject.TeamId != existProjectModel.AssignedTeam.TeamId)
                {
                    var existTeam = db.Teams.FirstOrDefault(x => x.TeamId == _existProject.TeamId);

                    ActivityRepository.ActivityCreator
                    ("changed " + _existProject.Name + " project team to the team " + existTeam.Name + ".", _existProject.ProjectId, null);
                }
                _existProject.Name = existProjectModel.Project.Name;
                _existProject.DayCount = existProjectModel.Project.DayCount;
                if(existProjectModel.Project.EndDate > existProjectModel.Project.CreatedDate)
                {
                    _existProject.EndDate = existProjectModel.Project.EndDate;
                    _existProject.DayCount = _existProject.EndDate.Subtract(DateTime.Now).Days;
                }
                //_existProject.EndDate = existProjectModel.Project.CreatedDate.AddDays(existProjectModel.Project.DayCount);
                _existProject.DefaultSprintTime = existProjectModel.Project.DefaultSprintTime;
                db.SaveChanges();

                ActivityRepository.ActivityCreator
                    ("edited " + _existProject.Name + " project.", _existProject.ProjectId, null);

                return RedirectToAction("Index", "Project", new { id = existProjectModel.Project.TeamId });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult IndexEpic(int id)
        {
            List<Epic> Epics = new List<Epic>();
            Epics = EpicRepository.GetEpics(id);
            if (Epics.Count == 0)
            {
                var pTask = new Epic();
                pTask.ProjectId = id;
                Epics.Add(pTask);
            }
            return View(Epics);
        }

        public ActionResult CommentBacklog(int backlogId, int? from)
        {
            var newComment = new Comment();
            newComment.ProductBacklogId = backlogId;
            var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
            newComment.MemberId = TeamRepository.GetUserMemberIdFromProjectId(existBacklog.ProjectId);

            

            return View(newComment);
        }

        [HttpPost]
        public ActionResult CommentBacklog(Comment commentModel)
        {
            var newComment = new Comment();
            newComment.ProductBacklogId = commentModel.ProductBacklogId;
            newComment.MemberId = commentModel.MemberId;
            var existMember = db.Members.FirstOrDefault(x => x.MemberId == newComment.MemberId);
            newComment.MemberShortName = existMember.Name;
            newComment.MemberComment = commentModel.MemberComment;
            newComment.CreatedDate = DateTime.Now;
            db.Comments.Add(newComment);
            db.SaveChanges();

            var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == commentModel.ProductBacklogId);
            ActivityRepository.ActivityCreator
                   ("commented " + existBacklog.Name + " backlog.", existBacklog.ProjectId, existBacklog.ProductBacklogId);

            return RedirectToAction("EditBacklog", new { id = newComment.ProductBacklogId });
        }

        public ActionResult EditBacklog(int id, int from = 0, int sprintNo = 0, int sortBy = 0)
        {
            var backlogModel = ProjectRepository.GetBacklogViewModel(id);
            backlogModel.ViewSortBy = sortBy;
            backlogModel.ViewSprintNo = sprintNo;

            return View(backlogModel);
        }
        [HttpPost]
        public ActionResult EditBacklog(BacklogViewModel backlogModel)
        {
            var _existBacklog = new ProductBacklog();
            _existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId 
            == backlogModel.Backlog.ProductBacklogId);
            _existBacklog.Name = backlogModel.Backlog.Name;
            _existBacklog.Priority = backlogModel.Backlog.Priority;
            _existBacklog.StoryPoint = backlogModel.Backlog.StoryPoint;
            _existBacklog.AcceptanceCriteria = backlogModel.Backlog.AcceptanceCriteria;
            _existBacklog.EpicId = backlogModel.Backlog.EpicId;
            _existBacklog.SprintNo = backlogModel.Backlog.SprintNo;
            _existBacklog.BacklogStatus = backlogModel.Backlog.BacklogStatus;

            if(backlogModel.AssignMember != null)
            {
                var existMember = db.Members.FirstOrDefault
                    (x => x.MemberId == backlogModel.AssignMember.MemberId);
                var backlogToMember = new BacklogToMember();
                if(existMember != null && backlogToMember != null)
                {
                    backlogToMember.MemberId = existMember.MemberId;
                    backlogToMember.MemberName = existMember.Name;
                    backlogToMember.ProductBacklogId = backlogModel.Backlog.ProductBacklogId;
                    db.BacklogToMembers.Add(backlogToMember);
                }
            }
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("edited " + _existBacklog.Name + " backlog.", _existBacklog.ProjectId, _existBacklog.ProductBacklogId);
            return RedirectToAction("EditBacklog", new { id = _existBacklog.ProductBacklogId ,
                sprintNo = backlogModel.ViewSprintNo , sortBy = backlogModel.ViewSortBy });
        }

        public ActionResult ChangeStatus(int backlogId, int status, int sprintNo = 0, int sortBy = 0)
        {
            var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
            existBacklog.BacklogStatus = (Status)status;
            db.SaveChanges();
            return RedirectToAction("IndexBacklog", new {projectId = existBacklog.ProjectId, sprintNo, sortBy, status});
        }
        public ActionResult AssignBacklog(int memberId, int backlogId, int? from)
        {
            if(UserRepository.IsUserSigned())
            {
                var _backlogToMember = new BacklogToMember();
                var existBacklog = new ProductBacklog();
                existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
                var existProject = new Project();
                existProject = db.Projects.FirstOrDefault(x => x.ProjectId == existBacklog.ProjectId);
                if(existProject != null)
                {
                    if(memberId != 0)
                    {
                        var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                        var existAssignment = db.BacklogToMembers.FirstOrDefault
                            (x => x.ProductBacklogId == existBacklog.ProductBacklogId && x.MemberId == memberId);
                        if(existAssignment !=null)
                        {
                            return Content("Member already assigned that backlog.");
                        }
                        else
                        {
                            _backlogToMember.ProductBacklogId = backlogId;
                            _backlogToMember.MemberId = memberId;
                            _backlogToMember.MemberName = existMember.Name;
                            db.BacklogToMembers.Add(_backlogToMember);
                            db.SaveChanges();

                            ActivityRepository.ActivityCreator
                   ("assigned " + existMember.Name + " to the backlog "+ existBacklog.Name, 
                   existBacklog.ProjectId, existBacklog.ProductBacklogId);

                            return RedirectToAction("EditBacklog", new { id = backlogId });
                        }
                        
                    }
                    
                }
                return Content("There are errors, you cannot assign to this backlog.");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult UnAssignUserFromEditBacklog(int backlogId, int memberId)
        {
            if (UserRepository.IsUserSigned())
            {
                var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlogId);
                if (existBacklog != null)
                {
                    if (memberId != 0)
                    {
                        ProjectRepository.UnAssignUser(backlogId, memberId);
                        var existMember = db.Members.FirstOrDefault(x => x.MemberId == memberId);
                        ActivityRepository.ActivityCreator
                   ("unassigned " + existMember.Name + " to the backlog " + existBacklog.Name,
                   existBacklog.ProjectId, existBacklog.ProductBacklogId);

                        return RedirectToAction("EditBacklog", new { id = existBacklog.ProductBacklogId });
                    }
                }
                return Content("There are errors, you cannot unassign from this backlog.");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult GiveIn(int id, int? from)
        {
            if(UserRepository.IsUserSigned())
            {
                var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
                if(existBacklog != null)
                {
                    int memberId = TeamRepository.GetUserMemberIdFromProjectId(existBacklog.ProjectId);
                    if(memberId != 0)
                    {
                        ProjectRepository.UnAssignUser(id, memberId);

                        ActivityRepository.ActivityCreator
                   ("unassigned yourself from " + existBacklog.Name + " backlog.",
                   existBacklog.ProjectId, existBacklog.ProductBacklogId);

                        return RedirectToAction("ProjectHome", new { id = existBacklog.ProjectId });
                    }
                }
                return Content("There are errors, you cannot unassign from this backlog.");
            }
            return RedirectToAction("Login", "User");

        }

        public ActionResult CreateBacklog(int projectId, int sprintNo = 0,int sortBy = 0)
        {
            var backlogModel = new BacklogViewModel();
            backlogModel.Backlog.ProjectId = projectId;
            backlogModel.ProjectEpics = EpicRepository.GetEpics(projectId);
            backlogModel.Epic.ProjectId = backlogModel.Backlog.ProjectId;

            backlogModel.ViewSortBy = sortBy;
            backlogModel.ViewSprintNo = sprintNo;
            return View(backlogModel);
        }

        [HttpPost]
        public ActionResult CreateBacklog(BacklogViewModel backlogModel)
        {
            var backlogEpic = db.Epics.FirstOrDefault(x => x.EpicId == backlogModel.Backlog.EpicId);
            if(backlogEpic != null)
            {
                backlogModel.Backlog.EpicName = backlogEpic.Name;
                db.ProductBacklogs.Add(backlogModel.Backlog);
                db.SaveChanges();

                ActivityRepository.ActivityCreator
                   ("created " + backlogModel.Backlog.Name + " backlog.",
                   backlogModel.Backlog.ProjectId, backlogModel.Backlog.ProductBacklogId);

                return RedirectToAction("EditBacklog", new { id = backlogModel.Backlog.ProductBacklogId, sprintNo = backlogModel.ViewSprintNo, sortBy = backlogModel.ViewSortBy });
            }

            return Content("Please assign a epic task first.");
        }

        public ActionResult ProjectHome(int id)
        {
            var projectViewModel = new ProjectViewModel();
            projectViewModel = ProjectRepository.PrepareProjectHome(id);
            if(projectViewModel != null)
            {
                return View(projectViewModel);
            }
            return Content("There is no project with that id.");
        }

        public ActionResult IndexBacklog(int projectId, int sprintNo = 0, int sortBy = 0)
        {
            if (UserRepository.IsUserSigned())
            {
                var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
                if (existProject != null)
                {
                    var projectHVM = new ProjectHomeViewModel
                    {
                        ProjectId = projectId,
                        ProjectName = existProject.Name,
                        TeamId = existProject.TeamId ?? default(int),
                        CurrentSprintNo = existProject.CurrentSprintNo,
                        TotalSprintCount = ProjectRepository.GetSprintCount(projectId),
                        ProductBacklogs = ProjectRepository.GetProjectBacklogs(projectId, sprintNo, sortBy),
                        ViewSprintNo = sprintNo,
                        ViewSortBy = sortBy
                    };

                    if (projectHVM.ProductBacklogs.Count == 0)
                    {
                        var backlog = new ProductBacklog();
                        backlog.ProjectId = projectId;
                        projectHVM.ProductBacklogs.Add(backlog);
                    }
                    return View(projectHVM);
                }
                return Content("Project cannot found.");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult IndexSprint(int projectId, int sprintNo)
        {

            var existProject = new Project();
            existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            var sprintModel = new SprintViewModel
            {
                ProjectId = projectId,
                ProjectCurrentSprint = existProject.CurrentSprintNo,
                SprintCount = ProjectRepository.GetSprintCount(projectId),
                SprintNo = sprintNo,
                SprintBacklogs = ProjectRepository.GetSprintBacklogs(projectId, sprintNo)
            };



            return View(sprintModel);
        }

        public ActionResult CreateEpic(int id, int? from)
        {
            var newEpic = new Epic();
            newEpic.ProjectId = id;

            return View(newEpic);
        }

        [HttpPost]
        public ActionResult CreateEpic(Epic newEpic)
        {
            db.Epics.Add(newEpic);
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("created " + newEpic.Name + " epic.",
                   newEpic.ProjectId, null);

            return RedirectToAction("IndexEpic", new { id = newEpic.ProjectId });
        }

        public ActionResult DeleteEpic(int id, int? from)
        {
            var existTask = new Epic();
            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
            db.Epics.Remove(existTask);
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("deleted " + existTask.Name + " epic.",
                   existTask.ProjectId, null);

            return RedirectToAction("IndexEpic",new { id = existTask.ProjectId });
        }

        public ActionResult EditEpic(int id, int? from)
        {
            Epic existTask = new Epic();
            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);

            return View(existTask);
        }

        [HttpPost]
        public ActionResult EditEpic(Epic existTask)
        {
            var _existTask = new Epic();
            _existTask = db.Epics.FirstOrDefault(x => x.EpicId == existTask.EpicId);
            _existTask.Name = existTask.Name;
            _existTask.Priority = existTask.Priority;
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("edited " + _existTask.Name + " epic.",
                   _existTask.ProjectId, null);

            return RedirectToAction("IndexEpic", new { id = existTask.ProjectId });
        }

        public ActionResult CompleteBacklog(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                if(ProjectRepository.CompleteBacklog(id,null))
                {
                    var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);

                    ActivityRepository.ActivityCreator
                   ("completed " + existBacklog.Name + " backlog.",
                   existBacklog.ProjectId, existBacklog.ProductBacklogId);

                    return RedirectToAction("IndexBacklog", "Project", new { projectId = existBacklog.ProjectId });

                }
                return Content("There are errors, you cannot unassign from this backlog.");
            }
            return RedirectToAction("Login", "User");
        }

        //Sprint ekranında tamamlama methodu, give in methodunu da yaz
        //public ActionResult CompleteBacklog(int id,int projectId, int sprintNo)
        //{
        //    if (UserRepository.IsUserSigned())
        //    {
        //        if (ProjectRepository.CompleteBacklog(id, null))
        //        {
        //            var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
        //            return RedirectToAction("Sprint", "Project", new { id = existBacklog.ProjectId });

        //        }
        //        return Content("There are errors, you cannot unassign from this backlog.");
        //    }
        //    return RedirectToAction("Login", "User");
        //}

        public ActionResult GreenEpic(int id)
        {
            var existTask = new Epic();
            existTask = db.Epics.FirstOrDefault(x => x.EpicId == id);
            existTask.IsDone = false;
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("uncompleted " + existTask.Name + " epic.",
                   existTask.ProjectId, null);

            return RedirectToAction("IndexEpic", "Project", new { id = existTask.ProjectId });
        }

        public ActionResult Delete (int id)
        {
            var existProject = new Project();
            existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            if (TeamRepository.IsUserManagerOfTeam(UserRepository.GetUserId(), existProject.TeamId ?? default (int)))
            {
                db.Projects.Remove(existProject);
                db.SaveChanges();

                ActivityRepository.ActivityCreator
                   ("deleted " + existProject.Name + " project.",
                   existProject.ProjectId, null);

                return RedirectToAction("Index", "Project", new { id = existProject.TeamId });
            }    
            
            return RedirectToAction("Login", "User");
            
        }

        public ActionResult Complete(int id)
        {
            var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            existProject.IsDone = true;
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("completed " + existProject.Name + " project.",
                   existProject.ProjectId, null);

            return RedirectToAction("Index", "Project", new { id = existProject.TeamId});
        }

        public ActionResult GreenProject(int id)
        {
            var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == id);
            existProject.IsDone = false;
            db.SaveChanges();

            ActivityRepository.ActivityCreator
                   ("uncompleted " + existProject.Name + " project.",
                   existProject.ProjectId, null);

            return RedirectToAction("Index", "Project", new { id = existProject.TeamId});
        }

        public ActionResult GreenBacklog(int id)
        {
            if(UserRepository.IsUserSigned())
            {
                var existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == id);
                if(TeamRepository.IsProjectManager(existBacklog.ProjectId))
                {
                    existBacklog.Done = false;
                    db.SaveChanges();

                    ActivityRepository.ActivityCreator
                   ("uncompleted " + existBacklog.Name + " backlog.",
                   existBacklog.ProjectId, existBacklog.ProductBacklogId);

                    return RedirectToAction("IndexBacklog", "Project", new { projectId = existBacklog.ProjectId });
                }
                return Content("There are errors, you cannot green this backlog.");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult AllEpics(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                return RedirectToAction("IndexEpic", "Project", new { id = id });
            }

            return RedirectToAction("Login", "User");
        }

        public ActionResult BackToList(int id)
        {
            if (UserRepository.IsUserSigned())
            {
                return RedirectToAction("Index", "Project", new { id = id});
            }

            return RedirectToAction("Login", "User");
        }
        
        public ActionResult EndSprint(int projectId, int sprintNo = 0, int sortBy = 0)
        {
            int userId = UserRepository.GetUserId();
            var existProject = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);

            int teamId = existProject.TeamId ?? default(int);
            if(TeamRepository.IsUserManagerOfTeam(userId, teamId))
            {
                List<ProductBacklog> existBacklogs = new List<ProductBacklog>();
                existBacklogs = ProjectRepository.GetProjectBacklogs(projectId);
                if (existProject.CurrentSprintNo < ProjectRepository.GetProjectSprintCount(projectId))
                {
                    existProject.CurrentSprintNo += 1;
                    foreach (var backlog in existBacklogs)
                    {
                        if (!backlog.Done && backlog.SprintNo < existProject.CurrentSprintNo)
                        {
                            var _existBacklog = db.ProductBacklogs.FirstOrDefault(x => x.ProductBacklogId == backlog.ProductBacklogId);
                            _existBacklog.SprintNo = existProject.CurrentSprintNo;

                        }
                    }
                    db.SaveChanges();
                }
                else if(existProject.CurrentSprintNo == ProjectRepository.GetProjectSprintCount(projectId))
                {
                    return Content("There is no next sprint.");
                }
                


                return RedirectToAction("IndexBacklog", new { projectId , sprintNo,  sortBy });
            }
            else
            {
                return  Content("You cannot do this operation");
            }
        }
        
    }
}