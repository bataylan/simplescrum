namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentySix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SprintTasks", "UserId", "dbo.Users");
            DropIndex("dbo.SprintTasks", new[] { "UserId" });
            AddColumn("dbo.SprintTasks", "MemberId", c => c.Int());
            CreateIndex("dbo.SprintTasks", "MemberId");
            AddForeignKey("dbo.SprintTasks", "MemberId", "dbo.Members", "MemberId");
            DropColumn("dbo.SprintTasks", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SprintTasks", "UserId", c => c.Int());
            DropForeignKey("dbo.SprintTasks", "MemberId", "dbo.Members");
            DropIndex("dbo.SprintTasks", new[] { "MemberId" });
            DropColumn("dbo.SprintTasks", "MemberId");
            CreateIndex("dbo.SprintTasks", "UserId");
            AddForeignKey("dbo.SprintTasks", "UserId", "dbo.Users", "UserId");
        }
    }
}
