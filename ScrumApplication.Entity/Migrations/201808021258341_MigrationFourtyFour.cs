namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourtyFour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductBacklogs", "EpicName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductBacklogs", "EpicName");
        }
    }
}
