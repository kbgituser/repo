namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cities_in_mall_are_mandatory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Malls", "CityId", "dbo.Cities");
            DropIndex("dbo.Malls", new[] { "CityId" });
            AlterColumn("dbo.Malls", "CityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Malls", "CityId");
            AddForeignKey("dbo.Malls", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Malls", "CityId", "dbo.Cities");
            DropIndex("dbo.Malls", new[] { "CityId" });
            AlterColumn("dbo.Malls", "CityId", c => c.Guid());
            CreateIndex("dbo.Malls", "CityId");
            AddForeignKey("dbo.Malls", "CityId", "dbo.Cities", "CityId");
        }
    }
}
