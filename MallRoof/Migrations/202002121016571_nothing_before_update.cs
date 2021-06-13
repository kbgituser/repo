namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nothing_before_update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Malls", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Demands", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands");
            DropIndex("dbo.Malls", new[] { "CityId" });
            DropIndex("dbo.AspNetUsers", new[] { "Demand_DemandId" });
            DropIndex("dbo.Demands", new[] { "UserId" });
            DropColumn("dbo.Malls", "ParkingExists");
            DropColumn("dbo.Malls", "ParkingInsideExists");
            DropColumn("dbo.Malls", "ParkingPayment");
            DropColumn("dbo.Malls", "ParkingInsidePayment");
            DropColumn("dbo.Malls", "CityId");
            DropColumn("dbo.AspNetUsers", "Demand_DemandId");
            DropTable("dbo.Cities");
            DropTable("dbo.Demands");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Demands",
                c => new
                    {
                        DemandId = c.Guid(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PriceFrom = c.Double(nullable: false),
                        PriceTo = c.Double(nullable: false),
                        AreaFrom = c.Double(nullable: false),
                        AreaTo = c.Double(nullable: false),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.DemandId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            AddColumn("dbo.AspNetUsers", "Demand_DemandId", c => c.Guid());
            AddColumn("dbo.Malls", "CityId", c => c.Guid(nullable: false));
            AddColumn("dbo.Malls", "ParkingInsidePayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingPayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingInsideExists", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingExists", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Demands", "UserId");
            CreateIndex("dbo.AspNetUsers", "Demand_DemandId");
            CreateIndex("dbo.Malls", "CityId");
            AddForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands", "DemandId");
            AddForeignKey("dbo.Demands", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Malls", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
        }
    }
}
