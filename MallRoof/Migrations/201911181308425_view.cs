namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class view : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MallPhotoes",
                c => new
                    {
                        MallPhotoId = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                        MallId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.MallPhotoId)
                .ForeignKey("dbo.Malls", t => t.MallId, cascadeDelete: true)
                .Index(t => t.MallId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MallPhotoes", "MallId", "dbo.Malls");
            DropIndex("dbo.MallPhotoes", new[] { "MallId" });
            DropTable("dbo.MallPhotoes");
        }
    }
}
