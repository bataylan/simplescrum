namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwelve1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "DayCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "DayCount", c => c.Int());
        }
    }
}
