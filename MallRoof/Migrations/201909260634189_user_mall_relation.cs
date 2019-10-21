namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_mall_relation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Mall_MallId", "dbo.Malls");
            DropIndex("dbo.AspNetUsers", new[] { "Mall_MallId" });
            CreateTable(
                "dbo.MallUsers",
                c => new
                    {
                        Mall_MallId = c.Guid(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Mall_MallId, t.User_Id })
                .ForeignKey("dbo.Malls", t => t.Mall_MallId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Mall_MallId)
                .Index(t => t.User_Id);
            
            DropColumn("dbo.AspNetUsers", "Mall_MallId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Mall_MallId", c => c.Guid());
            DropForeignKey("dbo.MallUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MallUsers", "Mall_MallId", "dbo.Malls");
            DropIndex("dbo.MallUsers", new[] { "User_Id" });
            DropIndex("dbo.MallUsers", new[] { "Mall_MallId" });
            DropTable("dbo.MallUsers");
            CreateIndex("dbo.AspNetUsers", "Mall_MallId");
            AddForeignKey("dbo.AspNetUsers", "Mall_MallId", "dbo.Malls", "MallId");
        }
    }
}
