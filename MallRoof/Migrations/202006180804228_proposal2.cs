namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Demands", "Demand_DemandId", "dbo.Demands");
            DropIndex("dbo.Demands", new[] { "Demand_DemandId" });
            DropColumn("dbo.Demands", "Demand_DemandId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Demands", "Demand_DemandId", c => c.Guid());
            CreateIndex("dbo.Demands", "Demand_DemandId");
            AddForeignKey("dbo.Demands", "Demand_DemandId", "dbo.Demands", "DemandId");
        }
    }
}
