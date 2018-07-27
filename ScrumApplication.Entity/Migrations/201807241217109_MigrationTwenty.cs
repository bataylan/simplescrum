namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwenty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "TotalPoint", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TotalPoint");
        }
    }
}
