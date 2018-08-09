namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFiftyOne : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductBacklogs", "Description", c => c.String());
            AddColumn("dbo.Users", "LastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.ProductBacklogs", "Description");
        }
    }
}
