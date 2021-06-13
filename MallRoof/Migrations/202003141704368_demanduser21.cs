namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demanduser21 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Proposals", new[] { "UserId" });
            AlterColumn("dbo.Proposals", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Proposals", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Proposals", new[] { "UserId" });
            AlterColumn("dbo.Proposals", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Proposals", "UserId");
        }
    }
}
