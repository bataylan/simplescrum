namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TwentyTwo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SprintTasks", "ProjectId", c => c.Int());
            CreateIndex("dbo.SprintTasks", "ProjectId");
            AddForeignKey("dbo.SprintTasks", "ProjectId", "dbo.Projects", "ProjectId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SprintTasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.SprintTasks", new[] { "ProjectId" });
            DropColumn("dbo.SprintTasks", "ProjectId");
        }
    }
}
