namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class instagramphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Premises", "InstaPhoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Premises", "InstaPhoto");
        }
    }
}
