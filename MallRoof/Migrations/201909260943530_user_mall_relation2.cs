namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_mall_relation2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Premises", "Mall_MallId", "dbo.Malls");
            DropIndex("dbo.Premises", new[] { "Mall_MallId" });
            RenameColumn(table: "dbo.Premises", name: "Mall_MallId", newName: "MallId");
            AlterColumn("dbo.Premises", "MallId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Premises", "MallId");
            AddForeignKey("dbo.Premises", "MallId", "dbo.Malls", "MallId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Premises", "MallId", "dbo.Malls");
            DropIndex("dbo.Premises", new[] { "MallId" });
            AlterColumn("dbo.Premises", "MallId", c => c.Guid());
            RenameColumn(table: "dbo.Premises", name: "MallId", newName: "Mall_MallId");
            CreateIndex("dbo.Premises", "Mall_MallId");
            AddForeignKey("dbo.Premises", "Mall_MallId", "dbo.Malls", "MallId");
        }
    }
}
