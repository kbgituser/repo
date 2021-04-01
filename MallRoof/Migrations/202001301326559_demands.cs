namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class demands : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Demands",
                c => new
                    {
                        DemandId = c.Guid(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        PriceFrom = c.Double(nullable: false),
                        PriceTo = c.Double(nullable: false),
                        AreaFrom = c.Double(nullable: false),
                        AreaTo = c.Double(nullable: false),
                        Message = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DemandId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Demands", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Demands", new[] { "User_Id" });
            DropTable("dbo.Demands");
        }
    }
}
