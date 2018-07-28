namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class thirty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "ManagerUserId", c => c.Int(nullable: false));
            AddColumn("dbo.Sprints", "ManagerUserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sprints", "ManagerUserId");
            DropColumn("dbo.Projects", "ManagerUserId");
        }
    }
}
