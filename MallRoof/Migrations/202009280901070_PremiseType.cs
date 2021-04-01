namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PremiseType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PremiseTypes",
                c => new
                    {
                        PremiseTypeId = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PremiseTypeId);
            
            AddColumn("dbo.Demands", "PremiseTypeId", c => c.Guid());
            CreateIndex("dbo.Demands", "PremiseTypeId");
            AddForeignKey("dbo.Demands", "PremiseTypeId", "dbo.PremiseTypes", "PremiseTypeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demands", "PremiseTypeId", "dbo.PremiseTypes");
            DropIndex("dbo.Demands", new[] { "PremiseTypeId" });
            DropColumn("dbo.Demands", "PremiseTypeId");
            DropTable("dbo.PremiseTypes");
        }
    }
}
