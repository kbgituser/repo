namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
            AddColumn("dbo.Malls", "CityId", c => c.Guid());
            CreateIndex("dbo.Malls", "CityId");
            AddForeignKey("dbo.Malls", "CityId", "dbo.Cities", "CityId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Malls", "CityId", "dbo.Cities");
            DropIndex("dbo.Malls", new[] { "CityId" });
            DropColumn("dbo.Malls", "CityId");
            DropTable("dbo.Cities");
        }
    }
}
