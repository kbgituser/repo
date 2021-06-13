namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demandCity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "CityId", c => c.Guid());
            CreateIndex("dbo.Demands", "CityId");
            AddForeignKey("dbo.Demands", "CityId", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demands", "CityId", "dbo.Cities");
            DropIndex("dbo.Demands", new[] { "CityId" });
            DropColumn("dbo.Demands", "CityId");
        }
    }
}
