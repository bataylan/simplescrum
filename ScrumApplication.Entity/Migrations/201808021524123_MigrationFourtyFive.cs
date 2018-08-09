namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourtyFive : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BacklogToMembers",
                c => new
                    {
                        BacklogToMemberId = c.Int(nullable: false, identity: true),
                        ProductBacklogId = c.Int(nullable: false),
                        MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BacklogToMemberId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .ForeignKey("dbo.ProductBacklogs", t => t.ProductBacklogId, cascadeDelete: true)
                .Index(t => t.ProductBacklogId)
                .Index(t => t.MemberId);
            
            AddColumn("dbo.Managers", "MemberId", c => c.Int());
            CreateIndex("dbo.Managers", "MemberId");
            AddForeignKey("dbo.Managers", "MemberId", "dbo.Members", "MemberId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BacklogToMembers", "ProductBacklogId", "dbo.ProductBacklogs");
            DropForeignKey("dbo.BacklogToMembers", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Managers", "MemberId", "dbo.Members");
            DropIndex("dbo.Managers", new[] { "MemberId" });
            DropIndex("dbo.BacklogToMembers", new[] { "MemberId" });
            DropIndex("dbo.BacklogToMembers", new[] { "ProductBacklogId" });
            DropColumn("dbo.Managers", "MemberId");
            DropTable("dbo.BacklogToMembers");
        }
    }
}
