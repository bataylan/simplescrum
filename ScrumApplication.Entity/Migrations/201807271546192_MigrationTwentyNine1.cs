namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentyNine1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SprintTasks", "AssignedTo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SprintTasks", "AssignedTo");
        }
    }
}
