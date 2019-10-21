namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_id_string : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Malls", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Malls", new[] { "User_Id" });
            DropColumn("dbo.Malls", "UserId");
            RenameColumn(table: "dbo.Malls", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Malls", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Malls", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Malls", "UserId");
            AddForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Malls", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Malls", new[] { "UserId" });
            AlterColumn("dbo.Malls", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Malls", "UserId", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.Malls", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Malls", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Malls", "User_Id");
            AddForeignKey("dbo.Malls", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
