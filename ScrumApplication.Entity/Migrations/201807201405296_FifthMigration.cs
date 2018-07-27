namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FifthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sprints", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sprints", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sprints", "EndDate");
            DropColumn("dbo.Sprints", "CreatedDate");
        }
    }
}
