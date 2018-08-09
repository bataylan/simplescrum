namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourtyEight : DbMigration
    {
        public override void Up()
        {   
            AddColumn("dbo.BacklogToMembers", "MemberName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BacklogToMembers", "MemberName");
        }
    }
}
