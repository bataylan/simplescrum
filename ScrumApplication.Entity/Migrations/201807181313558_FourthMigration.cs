namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "TeamId", "dbo.Teams");
            DropIndex("dbo.Projects", new[] { "TeamId" });
            RenameColumn(table: "dbo.Projects", name: "TeamId", newName: "Team_TeamId");
            AlterColumn("dbo.Projects", "Team_TeamId", c => c.Int());
            CreateIndex("dbo.Projects", "Team_TeamId");
            AddForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams", "TeamId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Projects", new[] { "Team_TeamId" });
            AlterColumn("dbo.Projects", "Team_TeamId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Projects", name: "Team_TeamId", newName: "TeamId");
            CreateIndex("dbo.Projects", "TeamId");
            AddForeignKey("dbo.Projects", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
        }
    }
}
