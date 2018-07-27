namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentyEight : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Managers", "CompanyId", c => c.Int());
            CreateIndex("dbo.Managers", "CompanyId");
            AddForeignKey("dbo.Managers", "CompanyId", "dbo.Companies", "CompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Managers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Managers", new[] { "CompanyId" });
            DropColumn("dbo.Managers", "CompanyId");
        }
    }
}
