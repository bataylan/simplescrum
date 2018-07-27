namespace ScrumApplication.Entity.DbContext
{
    using ScrumApplication.Entity.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ScrumApplicationDbContext : DbContext
    {
        // Your context has been configured to use a 'ScrumApplicationDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'ScrumApplication.Entity.DbContext.ScrumApplicationDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ScrumApplicationDbContext' 
        // connection string in the application configuration file.
        public ScrumApplicationDbContext()
            : base("name=ScrumApplicationDbContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectTask> ProjectTasks { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<SprintTask> SprintTasks { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Member> Members { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}