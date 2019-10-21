namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "Premise_PremiseId", "dbo.Premises");
            DropIndex("dbo.Photos", new[] { "Premise_PremiseId" });
            RenameColumn(table: "dbo.Photos", name: "Premise_PremiseId", newName: "PremiseId");
            AlterColumn("dbo.Photos", "PremiseId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Photos", "PremiseId");
            AddForeignKey("dbo.Photos", "PremiseId", "dbo.Premises", "PremiseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "PremiseId", "dbo.Premises");
            DropIndex("dbo.Photos", new[] { "PremiseId" });
            AlterColumn("dbo.Photos", "PremiseId", c => c.Guid());
            RenameColumn(table: "dbo.Photos", name: "PremiseId", newName: "Premise_PremiseId");
            CreateIndex("dbo.Photos", "Premise_PremiseId");
            AddForeignKey("dbo.Photos", "Premise_PremiseId", "dbo.Premises", "PremiseId");
        }
    }
}
