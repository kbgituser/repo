namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "EndDate");
        }
    }
}
