namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationNineteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sprints", "DefaultTime", c => c.Int(nullable: false));
            DropColumn("dbo.Sprints", "RemainingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sprints", "RemainingTime", c => c.Int(nullable: false));
            DropColumn("dbo.Sprints", "DefaultTime");
        }
    }
}
