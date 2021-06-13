namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phone_demand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "Phone");
        }
    }
}
