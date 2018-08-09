namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourtyTwo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductBacklogs", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProductBacklogs", new[] { "ProjectId" });
            AlterColumn("dbo.ProductBacklogs", "ProjectId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductBacklogs", "ProjectId");
            AddForeignKey("dbo.ProductBacklogs", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductBacklogs", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProductBacklogs", new[] { "ProjectId" });
            AlterColumn("dbo.ProductBacklogs", "ProjectId", c => c.Int());
            CreateIndex("dbo.ProductBacklogs", "ProjectId");
            AddForeignKey("dbo.ProductBacklogs", "ProjectId", "dbo.Projects", "ProjectId");
        }
    }
}
