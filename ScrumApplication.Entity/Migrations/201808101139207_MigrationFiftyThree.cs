namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFiftyThree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        ActivityDescription = c.String(),
                        ProjectId = c.Int(),
                        BacklogId = c.Int(),
                        MemberId = c.Int(),
                        ProductBacklog_ProductBacklogId = c.Int(),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.ProductBacklogs", t => t.ProductBacklog_ProductBacklogId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.MemberId)
                .Index(t => t.ProductBacklog_ProductBacklogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Activities", "ProductBacklog_ProductBacklogId", "dbo.ProductBacklogs");
            DropForeignKey("dbo.Activities", "MemberId", "dbo.Members");
            DropIndex("dbo.Activities", new[] { "ProductBacklog_ProductBacklogId" });
            DropIndex("dbo.Activities", new[] { "MemberId" });
            DropIndex("dbo.Activities", new[] { "ProjectId" });
            DropTable("dbo.Activities");
        }
    }
}
