namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposals", "PremiseId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Proposals", "PremiseId");
            AddForeignKey("dbo.Proposals", "PremiseId", "dbo.Premises", "PremiseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Proposals", "PremiseId", "dbo.Premises");
            DropIndex("dbo.Proposals", new[] { "PremiseId" });
            DropColumn("dbo.Proposals", "PremiseId");
        }
    }
}
