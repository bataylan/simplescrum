namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThirdMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SprintTasks", "ProjectTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.SprintTasks", new[] { "ProjectTaskId" });
            RenameColumn(table: "dbo.SprintTasks", name: "ProjectTaskId", newName: "ProjectTask_ProjectTaskId");
            RenameColumn(table: "dbo.SprintTasks", name: "UserId", newName: "User_UserId");
            RenameIndex(table: "dbo.SprintTasks", name: "IX_UserId", newName: "IX_User_UserId");
            AlterColumn("dbo.SprintTasks", "ProjectTask_ProjectTaskId", c => c.Int());
            CreateIndex("dbo.SprintTasks", "ProjectTask_ProjectTaskId");
            AddForeignKey("dbo.SprintTasks", "ProjectTask_ProjectTaskId", "dbo.ProjectTasks", "ProjectTaskId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SprintTasks", "ProjectTask_ProjectTaskId", "dbo.ProjectTasks");
            DropIndex("dbo.SprintTasks", new[] { "ProjectTask_ProjectTaskId" });
            AlterColumn("dbo.SprintTasks", "ProjectTask_ProjectTaskId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.SprintTasks", name: "IX_User_UserId", newName: "IX_UserId");
            RenameColumn(table: "dbo.SprintTasks", name: "User_UserId", newName: "UserId");
            RenameColumn(table: "dbo.SprintTasks", name: "ProjectTask_ProjectTaskId", newName: "ProjectTaskId");
            CreateIndex("dbo.SprintTasks", "ProjectTaskId");
            AddForeignKey("dbo.SprintTasks", "ProjectTaskId", "dbo.ProjectTasks", "ProjectTaskId", cascadeDelete: true);
        }
    }
}
