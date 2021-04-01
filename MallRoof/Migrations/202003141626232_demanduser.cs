namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demanduser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands");
            DropIndex("dbo.AspNetUsers", new[] { "Demand_DemandId" });
            CreateTable(
                "dbo.Proposals",
                c => new
                    {
                        ProposalId = c.Guid(nullable: false),
                        DemandId = c.Guid(nullable: false),
                        Price = c.Double(nullable: false),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProposalId)
                .ForeignKey("dbo.Demands", t => t.DemandId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.DemandId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Demands", "PossibleAddress", c => c.String());
            AddColumn("dbo.Demands", "Demand_DemandId", c => c.Guid());
            CreateIndex("dbo.Demands", "Demand_DemandId");
            AddForeignKey("dbo.Demands", "Demand_DemandId", "dbo.Demands", "DemandId");
            DropColumn("dbo.AspNetUsers", "Demand_DemandId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Demand_DemandId", c => c.Guid());
            DropForeignKey("dbo.Proposals", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Proposals", "DemandId", "dbo.Demands");
            DropForeignKey("dbo.Demands", "Demand_DemandId", "dbo.Demands");
            DropIndex("dbo.Proposals", new[] { "UserId" });
            DropIndex("dbo.Proposals", new[] { "DemandId" });
            DropIndex("dbo.Demands", new[] { "Demand_DemandId" });
            DropColumn("dbo.Demands", "Demand_DemandId");
            DropColumn("dbo.Demands", "PossibleAddress");
            DropTable("dbo.Proposals");
            CreateIndex("dbo.AspNetUsers", "Demand_DemandId");
            AddForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands", "DemandId");
        }
    }
}
