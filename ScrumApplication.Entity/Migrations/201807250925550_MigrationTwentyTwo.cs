namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentyTwo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sprints", "IsDone", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Projects", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "EndDate", c => c.DateTime());
            DropColumn("dbo.Sprints", "IsDone");
        }
    }
}
