namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentySeven1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "CompanyId", c => c.Int());
            CreateIndex("dbo.Members", "CompanyId");
            AddForeignKey("dbo.Members", "CompanyId", "dbo.Companies", "CompanyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Members", new[] { "CompanyId" });
            DropColumn("dbo.Members", "CompanyId");
        }
    }
}
