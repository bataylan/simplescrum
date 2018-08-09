namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourtyThree : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductBacklogs", "BacklogStatus", c => c.Int(nullable: false));
            DropColumn("dbo.ProductBacklogs", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductBacklogs", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.ProductBacklogs", "BacklogStatus");
        }
    }
}
