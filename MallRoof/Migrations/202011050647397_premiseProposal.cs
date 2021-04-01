namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class premiseProposal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceProposalToPremises",
                c => new
                    {
                        PriceProposalToPremiseId = c.Guid(nullable: false, identity: true),
                        ProposingUserId = c.String(maxLength: 128),
                        PremiseId = c.Guid(nullable: false),
                        Price = c.Single(nullable: false),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.PriceProposalToPremiseId)
                .ForeignKey("dbo.Premises", t => t.PremiseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ProposingUserId)
                .Index(t => t.ProposingUserId)
                .Index(t => t.PremiseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceProposalToPremises", "ProposingUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PriceProposalToPremises", "PremiseId", "dbo.Premises");
            DropIndex("dbo.PriceProposalToPremises", new[] { "PremiseId" });
            DropIndex("dbo.PriceProposalToPremises", new[] { "ProposingUserId" });
            DropTable("dbo.PriceProposalToPremises");
        }
    }
}
