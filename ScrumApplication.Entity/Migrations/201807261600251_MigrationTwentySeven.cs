namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentySeven : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Members", "TeamId", "dbo.Teams");
            DropIndex("dbo.Members", new[] { "TeamId" });
            AlterColumn("dbo.Members", "TeamId", c => c.Int(nullable: false));
            CreateIndex("dbo.Members", "TeamId");
            AddForeignKey("dbo.Members", "TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "TeamId", "dbo.Teams");
            DropIndex("dbo.Members", new[] { "TeamId" });
            AlterColumn("dbo.Members", "TeamId", c => c.Int());
            CreateIndex("dbo.Members", "TeamId");
            AddForeignKey("dbo.Members", "TeamId", "dbo.Teams", "TeamId");
        }
    }
}
