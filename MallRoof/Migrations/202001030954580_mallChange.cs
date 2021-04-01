namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mallChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Malls", "ParkingExists", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingInsideExists", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingPayment", c => c.Boolean(nullable: false));
            AddColumn("dbo.Malls", "ParkingInsidePayment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Malls", "ParkingInsidePayment");
            DropColumn("dbo.Malls", "ParkingPayment");
            DropColumn("dbo.Malls", "ParkingInsideExists");
            DropColumn("dbo.Malls", "ParkingExists");
        }
    }
}
