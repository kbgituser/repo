namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mall_price : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Malls", "Smprice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Malls", "Smprice");
        }
    }
}
