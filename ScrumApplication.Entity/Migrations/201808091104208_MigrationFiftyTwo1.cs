namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFiftyTwo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "MemberShortName", c => c.String());
            AddColumn("dbo.Comments", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "CreatedDate");
            DropColumn("dbo.Comments", "MemberShortName");
        }
    }
}
