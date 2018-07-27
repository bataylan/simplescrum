namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeventhMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProjects", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UserProjects", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Projects", new[] { "Team_TeamId" });
            DropIndex("dbo.UserProjects", new[] { "User_UserId" });
            DropIndex("dbo.UserProjects", new[] { "Project_ProjectId" });
            RenameColumn(table: "dbo.Projects", name: "Team_TeamId", newName: "TeamId");
            RenameColumn(table: "dbo.SprintTasks", name: "User_UserId", newName: "UserId");
            RenameIndex(table: "dbo.SprintTasks", name: "IX_User_UserId", newName: "IX_UserId");
            AddColumn("dbo.Projects", "User_UserId", c => c.Int());
            AlterColumn("dbo.Projects", "TeamId", c => c.Int(nullable: true));
            CreateIndex("dbo.Projects", "TeamId");
            CreateIndex("dbo.Projects", "User_UserId");
            AddForeignKey("dbo.Projects", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Projects", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
            DropTable("dbo.UserProjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Project_ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Project_ProjectId });
            
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Projects", "User_UserId", "dbo.Users");
            DropIndex("dbo.Projects", new[] { "User_UserId" });
            DropIndex("dbo.Projects", new[] { "TeamId" });
            AlterColumn("dbo.Projects", "TeamId", c => c.Int());
            DropColumn("dbo.Projects", "User_UserId");
            RenameIndex(table: "dbo.SprintTasks", name: "IX_UserId", newName: "IX_User_UserId");
            RenameColumn(table: "dbo.SprintTasks", name: "UserId", newName: "User_UserId");
            RenameColumn(table: "dbo.Projects", name: "TeamId", newName: "Team_TeamId");
            CreateIndex("dbo.UserProjects", "Project_ProjectId");
            CreateIndex("dbo.UserProjects", "User_UserId");
            CreateIndex("dbo.Projects", "Team_TeamId");
            AddForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams", "TeamId");
            AddForeignKey("dbo.UserProjects", "Project_ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.UserProjects", "User_UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
    }
}
