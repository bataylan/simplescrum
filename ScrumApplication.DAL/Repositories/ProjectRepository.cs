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
    public class ProjectRepository
    {
        public static void AddProject(string name)
        {
            var _newProject = new Project();
            _newProject.Name = name;

            using (var db = new ScrumApplicationDbContext())
            {
                db.Projects.Add(_newProject);
                db.SaveChanges();
            }
        }

        public static void UpdateProjectCookie(int id)
        {
            HttpCookie newCookie = new HttpCookie("LastProject", id.ToString());
            newCookie.Expires = DateTime.Now.AddDays(1d);
            HttpContext.Current.Response.SetCookie(newCookie);
        }

        public static int GetLastProjectId()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("LastProject");
            return Int32.Parse(cookie.Value);
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
    }
}
