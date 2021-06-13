namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Demands", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Demands", "CreateDate");
        }
    }
}
