namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Demands", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Demands", new[] { "User_Id" });
            DropColumn("dbo.Demands", "UserId");
            RenameColumn(table: "dbo.Demands", name: "User_Id", newName: "UserId");
            AddColumn("dbo.AspNetUsers", "Demand_DemandId", c => c.Guid());
            AlterColumn("dbo.Demands", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Demands", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Demand_DemandId");
            CreateIndex("dbo.Demands", "UserId");
            AddForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands", "DemandId");
            AddForeignKey("dbo.Demands", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demands", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Demand_DemandId", "dbo.Demands");
            DropIndex("dbo.Demands", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Demand_DemandId" });
            AlterColumn("dbo.Demands", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Demands", "UserId", c => c.Guid(nullable: false));
            DropColumn("dbo.AspNetUsers", "Demand_DemandId");
            RenameColumn(table: "dbo.Demands", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Demands", "UserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Demands", "User_Id");
            AddForeignKey("dbo.Demands", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
