namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class norm : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Cities",
            //    c => new
            //        {
            //            CityId = c.Guid(nullable: false, identity: true),
            //            Name = c.String(nullable: false),
            //        })
            //    .PrimaryKey(t => t.CityId);
            
            //CreateTable(
            //    "dbo.Demands",
            //    c => new
            //        {
            //            DemandId = c.Guid(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 128),
            //            PriceFrom = c.Double(nullable: false),
            //            PriceTo = c.Double(nullable: false),
            //            AreaFrom = c.Double(nullable: false),
            //            AreaTo = c.Double(nullable: false),
            //            Message = c.String(),
            //        })
            //    .PrimaryKey(t => t.DemandId)
            //    .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
            //    .Index(t => t.UserId);
            
            //AddColumn("dbo.AspNetUsers", "Demand_DemandId", c => c.Guid());
            //AddColumn("dbo.Malls", "ParkingExists", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Malls", "ParkingInsideExists", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Malls", "ParkingPayment", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Malls", "ParkingInsidePayment", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Malls", "CityId", c => c.Guid(nullable: false));
            //CreateIndex("dbo.Malls", "CityId");
            //CreateIndex("dbo.AspNetUsers", "Demand_DemandId");
            //AddForeignKey("dbo.Malls", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            //AddForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands", "DemandId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands");
            DropForeignKey("dbo.Demands", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Malls", "CityId", "dbo.Cities");
            DropIndex("dbo.Demands", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Demand_DemandId" });
            DropIndex("dbo.Malls", new[] { "CityId" });
            DropColumn("dbo.Malls", "CityId");
            DropColumn("dbo.Malls", "ParkingInsidePayment");
            DropColumn("dbo.Malls", "ParkingPayment");
            DropColumn("dbo.Malls", "ParkingInsideExists");
            DropColumn("dbo.Malls", "ParkingExists");
            DropColumn("dbo.AspNetUsers", "Demand_DemandId");
            DropTable("dbo.Demands");
            DropTable("dbo.Cities");
        }
    }
}
