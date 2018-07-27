namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentyNine : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "TotalPoint", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "TotalPoint");
        }
    }
}
