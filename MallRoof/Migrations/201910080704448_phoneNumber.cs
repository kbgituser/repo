namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class phoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Malls", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Malls", "PhoneNumber");
        }
    }
}
