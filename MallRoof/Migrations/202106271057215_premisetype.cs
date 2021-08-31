namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class premisetype : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Premises", "PremiseTypeId", c => c.Guid());
            CreateIndex("dbo.Premises", "PremiseTypeId");
            AddForeignKey("dbo.Premises", "PremiseTypeId", "dbo.PremiseTypes", "PremiseTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Premises", "PremiseTypeId", "dbo.PremiseTypes");
            DropIndex("dbo.Premises", new[] { "PremiseTypeId" });
            DropColumn("dbo.Premises", "PremiseTypeId");
        }
    }
}
