namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_mall : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MallUsers", "Mall_MallId", "dbo.Malls");
            DropForeignKey("dbo.MallUsers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.MallUsers", new[] { "Mall_MallId" });
            DropIndex("dbo.MallUsers", new[] { "User_Id" });
            AddColumn("dbo.Malls", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.Malls", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Malls", "User_Id");
            AddForeignKey("dbo.Malls", "User_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.MallUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MallUsers",
                c => new
                    {
                        Mall_MallId = c.Guid(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Mall_MallId, t.User_Id });
            
            DropForeignKey("dbo.Malls", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Malls", new[] { "User_Id" });
            DropColumn("dbo.Malls", "User_Id");
            DropColumn("dbo.Malls", "UserId");
            CreateIndex("dbo.MallUsers", "User_Id");
            CreateIndex("dbo.MallUsers", "Mall_MallId");
            AddForeignKey("dbo.MallUsers", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MallUsers", "Mall_MallId", "dbo.Malls", "MallId", cascadeDelete: true);
        }
    }
}
