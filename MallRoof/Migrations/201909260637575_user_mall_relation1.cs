namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_mall_relation1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Malls", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Malls", "UserId", c => c.Guid(nullable: false));
        }
    }
}
