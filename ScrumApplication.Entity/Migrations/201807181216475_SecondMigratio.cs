namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigratio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SprintTasks", "Done", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SprintTasks", "Done");
        }
    }
}
