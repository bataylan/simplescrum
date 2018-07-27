namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Interval = c.Int(nullable: true),
                        SprintTime = c.Int(defaultValue: 15),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        ProjectTaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false, defaultValue: 5),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectTaskId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.SprintTasks",
                c => new
                    {
                        SprintTaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false, defaultValue: 5),
                        Point = c.Int(nullable: false, defaultValue: 1),
                        Done = c.Boolean(nullable: false, defaultValue: false),
                        UserId = c.Int(),
                        SprintId = c.Int(nullable: false),
                        ProjectTaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SprintTaskId)
                .ForeignKey("dbo.ProjectTasks", t => t.ProjectTaskId, cascadeDelete: false)
                .ForeignKey("dbo.Sprints", t => t.SprintId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SprintId)
                .Index(t => t.ProjectTaskId);
            
            CreateTable(
                "dbo.Sprints",
                c => new
                    {
                        SprintId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RemainingTime = c.Int(nullable: false, defaultValue: 15),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SprintId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mail = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Project_ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Project_ProjectId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_ProjectId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Project_ProjectId);
            
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_TeamId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_TeamId, t.User_UserId })
                .ForeignKey("dbo.Teams", t => t.Team_TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Team_TeamId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.SprintTasks", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserProjects", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.UserProjects", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.SprintTasks", "SprintId", "dbo.Sprints");
            DropForeignKey("dbo.Sprints", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.SprintTasks", "ProjectTaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.TeamUsers", new[] { "User_UserId" });
            DropIndex("dbo.TeamUsers", new[] { "Team_TeamId" });
            DropIndex("dbo.UserProjects", new[] { "Project_ProjectId" });
            DropIndex("dbo.UserProjects", new[] { "User_UserId" });
            DropIndex("dbo.Sprints", new[] { "ProjectId" });
            DropIndex("dbo.SprintTasks", new[] { "ProjectTaskId" });
            DropIndex("dbo.SprintTasks", new[] { "SprintId" });
            DropIndex("dbo.SprintTasks", new[] { "UserId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "TeamId" });
            DropTable("dbo.TeamUsers");
            DropTable("dbo.UserProjects");
            DropTable("dbo.Teams");
            DropTable("dbo.Users");
            DropTable("dbo.Sprints");
            DropTable("dbo.SprintTasks");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Projects");
        }
    }
}
