namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "DemandStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "DemandStatus");
        }
    }
}
