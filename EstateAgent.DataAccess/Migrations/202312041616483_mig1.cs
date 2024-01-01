namespace EstateAgent.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutMedia",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SlideIndex = c.Int(),
                        MediaId = c.Long(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Media", t => t.MediaId)
                .Index(t => t.MediaId);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MediaName = c.String(),
                        MediaType = c.String(),
                        MediaUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.About",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Agent",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        MediaId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Media", t => t.MediaId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MediaId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        ProfilePhotoId = c.Long(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Media", t => t.ProfilePhotoId)
                .Index(t => t.ProfilePhotoId);
            
            CreateTable(
                "dbo.Blog",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Content = c.String(),
                        PostedDate = c.DateTime(nullable: false),
                        MediaId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Media", t => t.MediaId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.MediaId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ContactInfo",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityEnt",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        PasswordHash = c.String(),
                        PasswordSalt = c.String(),
                        UserName = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        ExpiryDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Property",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Address = c.String(),
                        AgentId = c.Long(nullable: false),
                        BedRoomCount = c.Int(nullable: false),
                        LivingRoomCount = c.Int(nullable: false),
                        ParkingCount = c.Int(nullable: false),
                        KitchenCount = c.Int(nullable: false),
                        Details = c.String(),
                        LocationLatitude = c.Double(),
                        LocationLongitude = c.Double(),
                        PropertySaleStatus = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PropertyMedia",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyId = c.Long(nullable: false),
                        SlideIndex = c.Int(),
                        MediaId = c.Long(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Media", t => t.MediaId)
                .ForeignKey("dbo.Property", t => t.PropertyId)
                .Index(t => t.PropertyId)
                .Index(t => t.MediaId);
            
            CreateTable(
                "dbo.Subscribe",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(),
                        SubscribedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Key = c.String(),
                        Value = c.String(),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Role = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.PropertyMedia", "PropertyId", "dbo.Property");
            DropForeignKey("dbo.PropertyMedia", "MediaId", "dbo.Media");
            DropForeignKey("dbo.IdentityEnt", "UserId", "dbo.User");
            DropForeignKey("dbo.Blog", "UserId", "dbo.User");
            DropForeignKey("dbo.Blog", "MediaId", "dbo.Media");
            DropForeignKey("dbo.Agent", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "ProfilePhotoId", "dbo.Media");
            DropForeignKey("dbo.Agent", "MediaId", "dbo.Media");
            DropForeignKey("dbo.AboutMedia", "MediaId", "dbo.Media");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.PropertyMedia", new[] { "MediaId" });
            DropIndex("dbo.PropertyMedia", new[] { "PropertyId" });
            DropIndex("dbo.IdentityEnt", new[] { "UserId" });
            DropIndex("dbo.Blog", new[] { "UserId" });
            DropIndex("dbo.Blog", new[] { "MediaId" });
            DropIndex("dbo.User", new[] { "ProfilePhotoId" });
            DropIndex("dbo.Agent", new[] { "MediaId" });
            DropIndex("dbo.Agent", new[] { "UserId" });
            DropIndex("dbo.AboutMedia", new[] { "MediaId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.SystemSettings");
            DropTable("dbo.Subscribe");
            DropTable("dbo.PropertyMedia");
            DropTable("dbo.Property");
            DropTable("dbo.IdentityEnt");
            DropTable("dbo.ContactInfo");
            DropTable("dbo.Blog");
            DropTable("dbo.User");
            DropTable("dbo.Agent");
            DropTable("dbo.About");
            DropTable("dbo.Media");
            DropTable("dbo.AboutMedia");
        }
    }
}
