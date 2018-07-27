namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationTwentyFive : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.TeamUsers", "User_UserId", "dbo.Users");
            DropIndex("dbo.Projects", new[] { "User_UserId" });
            DropIndex("dbo.TeamUsers", new[] { "Team_TeamId" });
            DropIndex("dbo.TeamUsers", new[] { "User_UserId" });
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Managers",
                c => new
                    {
                        ManagerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Mail = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ManagerId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Mail = c.String(nullable: false),
                        RoleCode = c.Int(nullable: false),
                        TeamId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberId)
                .ForeignKey("dbo.Teams", t => t.TeamId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.TeamId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Teams", "ManagerId", c => c.Int());
            AddColumn("dbo.Teams", "CompanyId", c => c.Int());
            CreateIndex("dbo.Teams", "ManagerId");
            CreateIndex("dbo.Teams", "CompanyId");
            AddForeignKey("dbo.Teams", "CompanyId", "dbo.Companies", "CompanyId");
            AddForeignKey("dbo.Teams", "ManagerId", "dbo.Managers", "ManagerId");
            DropColumn("dbo.Projects", "User_UserId");
            DropTable("dbo.TeamUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_TeamId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_TeamId, t.User_UserId });
            
            AddColumn("dbo.Projects", "User_UserId", c => c.Int());
            DropForeignKey("dbo.Members", "UserId", "dbo.Users");
            DropForeignKey("dbo.Members", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Managers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Teams", "ManagerId", "dbo.Managers");
            DropForeignKey("dbo.Teams", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Members", new[] { "UserId" });
            DropIndex("dbo.Members", new[] { "TeamId" });
            DropIndex("dbo.Managers", new[] { "UserId" });
            DropIndex("dbo.Teams", new[] { "CompanyId" });
            DropIndex("dbo.Teams", new[] { "ManagerId" });
            DropColumn("dbo.Teams", "CompanyId");
            DropColumn("dbo.Teams", "ManagerId");
            DropTable("dbo.Members");
            DropTable("dbo.Managers");
            DropTable("dbo.Companies");
            CreateIndex("dbo.TeamUsers", "User_UserId");
            CreateIndex("dbo.TeamUsers", "Team_TeamId");
            CreateIndex("dbo.Projects", "User_UserId");
            AddForeignKey("dbo.TeamUsers", "User_UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.TeamUsers", "Team_TeamId", "dbo.Teams", "TeamId", cascadeDelete: true);
            AddForeignKey("dbo.Projects", "User_UserId", "dbo.Users", "UserId");
        }
    }
}
