namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demand_window : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "HasWindow", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "HasWindow");
        }
    }
}
