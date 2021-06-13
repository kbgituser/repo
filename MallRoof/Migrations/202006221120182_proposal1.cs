namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class proposal1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proposals", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Proposals", "CreateDate");
        }
    }
}
