namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demandCity1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Demands", new[] { "CityId" });
            AlterColumn("dbo.Demands", "CityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Demands", "CityId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Demands", new[] { "CityId" });
            AlterColumn("dbo.Demands", "CityId", c => c.Guid());
            CreateIndex("dbo.Demands", "CityId");
        }
    }
}
