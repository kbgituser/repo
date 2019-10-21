namespace MallRoof.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Malls",
                c => new
                    {
                        MallId = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        UserId = c.Guid(nullable: false),
                        NumberOfFloors = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MallId);
            
            CreateTable(
                "dbo.Premises",
                c => new
                    {
                        PremiseId = c.Guid(nullable: false, identity: true),
                        Number = c.String(),
                        Floor = c.Int(nullable: false),
                        Area = c.Double(nullable: false),
                        IsLastFloor = c.Boolean(nullable: false),
                        HasWindow = c.Boolean(nullable: false),
                        Description = c.String(),
                        Price = c.Double(nullable: false),
                        IsSeen = c.Boolean(nullable: false),
                        Mall_MallId = c.Guid(),
                    })
                .PrimaryKey(t => t.PremiseId)
                .ForeignKey("dbo.Malls", t => t.Mall_MallId)
                .Index(t => t.Mall_MallId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Guid(nullable: false, identity: true),
                        Path = c.String(),
                        Premise_PremiseId = c.Guid(),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Premises", t => t.Premise_PremiseId)
                .Index(t => t.Premise_PremiseId);
            
            CreateTable(
                "dbo.PremiseCalendars",
                c => new
                    {
                        PremiseCalendarId = c.Guid(nullable: false, identity: true),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        Premise_PremiseId = c.Guid(),
                        Tenant_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PremiseCalendarId)
                .ForeignKey("dbo.Premises", t => t.Premise_PremiseId)
                .ForeignKey("dbo.AspNetUsers", t => t.Tenant_Id)
                .Index(t => t.Premise_PremiseId)
                .Index(t => t.Tenant_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        FirstName = c.String(),
                        SurName = c.String(),
                        Phone = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Mall_MallId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Malls", t => t.Mall_MallId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Mall_MallId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.PremiseCalendars", "Tenant_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Mall_MallId", "dbo.Malls");
            DropForeignKey("dbo.PremiseCalendars", "Premise_PremiseId", "dbo.Premises");
            DropForeignKey("dbo.Photos", "Premise_PremiseId", "dbo.Premises");
            DropForeignKey("dbo.Premises", "Mall_MallId", "dbo.Malls");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Mall_MallId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.PremiseCalendars", new[] { "Tenant_Id" });
            DropIndex("dbo.PremiseCalendars", new[] { "Premise_PremiseId" });
            DropIndex("dbo.Photos", new[] { "Premise_PremiseId" });
            DropIndex("dbo.Premises", new[] { "Mall_MallId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.PremiseCalendars");
            DropTable("dbo.Photos");
            DropTable("dbo.Premises");
            DropTable("dbo.Malls");
        }
    }
}
