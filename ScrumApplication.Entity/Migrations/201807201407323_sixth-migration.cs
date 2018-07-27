namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sixthmigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Sprints", "EndDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sprints", "EndDate", c => c.DateTime(nullable: false));
        }
    }
}
