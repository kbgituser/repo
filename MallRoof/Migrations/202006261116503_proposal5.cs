namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Demands", "Phone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Demands", "Phone", c => c.String());
        }
    }
}
