namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photos1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PremiseCalendars", "Premise_PremiseId", "dbo.Premises");
            DropIndex("dbo.PremiseCalendars", new[] { "Premise_PremiseId" });
            RenameColumn(table: "dbo.PremiseCalendars", name: "Premise_PremiseId", newName: "PremiseId");
            AlterColumn("dbo.PremiseCalendars", "PremiseId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PremiseCalendars", "PremiseId");
            AddForeignKey("dbo.PremiseCalendars", "PremiseId", "dbo.Premises", "PremiseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PremiseCalendars", "PremiseId", "dbo.Premises");
            DropIndex("dbo.PremiseCalendars", new[] { "PremiseId" });
            AlterColumn("dbo.PremiseCalendars", "PremiseId", c => c.Guid());
            RenameColumn(table: "dbo.PremiseCalendars", name: "PremiseId", newName: "Premise_PremiseId");
            CreateIndex("dbo.PremiseCalendars", "Premise_PremiseId");
            AddForeignKey("dbo.PremiseCalendars", "Premise_PremiseId", "dbo.Premises", "PremiseId");
        }
    }
}
