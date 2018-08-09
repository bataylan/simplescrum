namespace ScrumApplication.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationFourty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SprintTasks", "MemberId", "dbo.Members");
            DropForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Sprints", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectTasks", "SprintId", "dbo.Sprints");
            DropForeignKey("dbo.SprintTasks", "SprintId", "dbo.Sprints");
            DropForeignKey("dbo.SprintTasks", "ProjectId", "dbo.Projects");
            DropIndex("dbo.SprintTasks", new[] { "SprintId" });
            DropIndex("dbo.SprintTasks", new[] { "MemberId" });
            DropIndex("dbo.SprintTasks", new[] { "ProjectId" });
            DropIndex("dbo.ProjectTasks", new[] { "ProjectId" });
            DropIndex("dbo.ProjectTasks", new[] { "SprintId" });
            DropIndex("dbo.Sprints", new[] { "ProjectId" });
            CreateTable(
                "dbo.ProductBacklogs",
                c => new
                    {
                        ProductBacklogId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
                        Point = c.Int(nullable: false),
                        Done = c.Boolean(nullable: false),
                        AcceptanceCriteria = c.String(),
                        SprintNo = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        EpicId = c.Int(),
                    })
                .PrimaryKey(t => t.ProductBacklogId)
                .ForeignKey("dbo.Epics", t => t.EpicId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.EpicId);
            
            CreateTable(
                "dbo.Epics",
                c => new
                    {
                        EpicId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EpicId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.ProductBacklogMembers",
                c => new
                    {
                        ProductBacklog_ProductBacklogId = c.Int(nullable: false),
                        Member_MemberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductBacklog_ProductBacklogId, t.Member_MemberId })
                .ForeignKey("dbo.ProductBacklogs", t => t.ProductBacklog_ProductBacklogId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.Member_MemberId, cascadeDelete: true)
                .Index(t => t.ProductBacklog_ProductBacklogId)
                .Index(t => t.Member_MemberId);
            
            AddColumn("dbo.Projects", "CurrentSprintNo", c => c.Int(nullable: false));
            DropTable("dbo.SprintTasks");
            DropTable("dbo.ProjectTasks");
            DropTable("dbo.Sprints");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Sprints",
                c => new
                    {
                        SprintId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DefaultTime = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SprintId);
            
            CreateTable(
                "dbo.ProjectTasks",
                c => new
                    {
                        ProjectTaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
                        IsDone = c.Boolean(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        SprintId = c.Int(),
                    })
                .PrimaryKey(t => t.ProjectTaskId);
            
            CreateTable(
                "dbo.SprintTasks",
                c => new
                    {
                        SprintTaskId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Priority = c.Int(nullable: false),
                        Point = c.Int(nullable: false),
                        Done = c.Boolean(nullable: false),
                        AssignedTo = c.String(),
                        SprintId = c.Int(nullable: false),
                        MemberId = c.Int(),
                        ProjectId = c.Int(),
                    })
                .PrimaryKey(t => t.SprintTaskId);
            
            DropForeignKey("dbo.ProductBacklogMembers", "Member_MemberId", "dbo.Members");
            DropForeignKey("dbo.ProductBacklogMembers", "ProductBacklog_ProductBacklogId", "dbo.ProductBacklogs");
            DropForeignKey("dbo.ProductBacklogs", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Epics", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProductBacklogs", "EpicId", "dbo.Epics");
            DropIndex("dbo.ProductBacklogMembers", new[] { "Member_MemberId" });
            DropIndex("dbo.ProductBacklogMembers", new[] { "ProductBacklog_ProductBacklogId" });
            DropIndex("dbo.Epics", new[] { "ProjectId" });
            DropIndex("dbo.ProductBacklogs", new[] { "EpicId" });
            DropIndex("dbo.ProductBacklogs", new[] { "ProjectId" });
            DropColumn("dbo.Projects", "CurrentSprintNo");
            DropTable("dbo.ProductBacklogMembers");
            DropTable("dbo.Epics");
            DropTable("dbo.ProductBacklogs");
            CreateIndex("dbo.Sprints", "ProjectId");
            CreateIndex("dbo.ProjectTasks", "SprintId");
            CreateIndex("dbo.ProjectTasks", "ProjectId");
            CreateIndex("dbo.SprintTasks", "ProjectId");
            CreateIndex("dbo.SprintTasks", "MemberId");
            CreateIndex("dbo.SprintTasks", "SprintId");
            AddForeignKey("dbo.SprintTasks", "ProjectId", "dbo.Projects", "ProjectId");
            AddForeignKey("dbo.SprintTasks", "SprintId", "dbo.Sprints", "SprintId", cascadeDelete: true);
            AddForeignKey("dbo.ProjectTasks", "SprintId", "dbo.Sprints", "SprintId");
            AddForeignKey("dbo.Sprints", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.ProjectTasks", "ProjectId", "dbo.Projects", "ProjectId", cascadeDelete: true);
            AddForeignKey("dbo.SprintTasks", "MemberId", "dbo.Members", "MemberId");
        }
    }
}
