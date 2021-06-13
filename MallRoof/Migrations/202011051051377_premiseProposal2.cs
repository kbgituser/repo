namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class premiseProposal2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PriceProposalToPremises", "PremiseId", "dbo.Premises");
            AddForeignKey("dbo.PriceProposalToPremises", "PremiseId", "dbo.Premises", "PremiseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PriceProposalToPremises", "PremiseId", "dbo.Premises");
            AddForeignKey("dbo.PriceProposalToPremises", "PremiseId", "dbo.Premises", "PremiseId", cascadeDelete: true);
        }
    }
}
