namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFiftyTwo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        MemberComment = c.String(),
                        MemberId = c.Int(nullable: false),
                        ProductBacklogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.ProductBacklogs", t => t.ProductBacklogId, cascadeDelete: true)
                .Index(t => t.MemberId)
                .Index(t => t.ProductBacklogId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "ProductBacklogId", "dbo.ProductBacklogs");
            DropForeignKey("dbo.Comments", "MemberId", "dbo.Members");
            DropIndex("dbo.Comments", new[] { "ProductBacklogId" });
            DropIndex("dbo.Comments", new[] { "MemberId" });
            DropTable("dbo.Comments");
        }
    }
}
