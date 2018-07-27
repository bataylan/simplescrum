namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFifteen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "DayCount", c => c.Int());
            DropColumn("dbo.Projects", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "EndDate", c => c.Int());
            DropColumn("dbo.Projects", "DayCount");
        }
    }
}
