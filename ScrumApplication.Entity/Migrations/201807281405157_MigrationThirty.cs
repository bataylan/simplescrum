namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationThirty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Projects", "ManagerUserId");
            DropColumn("dbo.Sprints", "ManagerUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sprints", "ManagerUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Projects", "ManagerUserId", c => c.Int(nullable: false));
        }
    }
}
