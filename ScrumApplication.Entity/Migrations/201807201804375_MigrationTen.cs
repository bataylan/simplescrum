namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTen : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SprintTasks", "ProjectTask_ProjectTaskId", "dbo.ProjectTasks");
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropIndex("dbo.Projects", new[] { "TeamId" });
            DropIndex("dbo.SprintTasks", new[] { "ProjectTask_ProjectTaskId" });
            AddColumn("dbo.Projects", "EndDate", c => c.Int());
            AddColumn("dbo.Projects", "DefaultSprintTime", c => c.Int(nullable: false));
            AddColumn("dbo.ProjectTasks", "IsDone", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProjectTasks", "SprintId", c => c.Int());
            AddColumn("dbo.Sprints", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sprints", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "TeamId", c => c.Int(nullable: true));
            CreateIndex("dbo.Projects", "TeamId");
            CreateIndex("dbo.ProjectTasks", "SprintId");
            AddForeignKey("dbo.ProjectTasks", "SprintId", "dbo.Sprints", "SprintId");
            AddForeignKey("dbo.Projects", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
            DropColumn("dbo.Projects", "Interval");
            DropColumn("dbo.Projects", "SprintTime");
            DropColumn("dbo.SprintTasks", "ProjectTask_ProjectTaskId");
            DropColumn("dbo.Sprints", "CreatedDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sprints", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SprintTasks", "ProjectTask_ProjectTaskId", c => c.Int());
            AddColumn("dbo.Projects", "SprintTime", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "Interval", c => c.Int());
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.ProjectTasks", "SprintId", "dbo.Sprints");
            DropIndex("dbo.ProjectTasks", new[] { "SprintId" });
            DropIndex("dbo.Projects", new[] { "TeamId" });
            AlterColumn("dbo.Projects", "TeamId", c => c.Int());
            DropColumn("dbo.Sprints", "EndDate");
            DropColumn("dbo.Sprints", "StartDate");
            DropColumn("dbo.ProjectTasks", "SprintId");
            DropColumn("dbo.ProjectTasks", "IsDone");
            DropColumn("dbo.Projects", "DefaultSprintTime");
            DropColumn("dbo.Projects", "EndDate");
            CreateIndex("dbo.SprintTasks", "ProjectTask_ProjectTaskId");
            CreateIndex("dbo.Projects", "TeamId");
            AddForeignKey("dbo.Projects", "TeamId", "dbo.Teams", "TeamId");
            AddForeignKey("dbo.SprintTasks", "ProjectTask_ProjectTaskId", "dbo.ProjectTasks", "ProjectTaskId");
        }
    }
}
