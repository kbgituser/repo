namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mall_has_required_user_____Photo_premises : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Malls", new[] { "UserId" });
            AlterColumn("dbo.Malls", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Malls", "UserId");
            AddForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Malls", new[] { "UserId" });
            AlterColumn("dbo.Malls", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Malls", "UserId");
            AddForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
