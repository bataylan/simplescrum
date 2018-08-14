namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFiftyThree1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "CreatedTime");
        }
    }
}
