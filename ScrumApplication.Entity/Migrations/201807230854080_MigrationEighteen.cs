namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationEighteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "EndDate", c => c.DateTime());
            AddColumn("dbo.Projects", "isDone", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "isDone");
            DropColumn("dbo.Projects", "EndDate");
            DropColumn("dbo.Projects", "CreatedDate");
        }
    }
}
