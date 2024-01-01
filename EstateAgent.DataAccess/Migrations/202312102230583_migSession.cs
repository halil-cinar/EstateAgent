namespace EstateAgent.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migSession : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IdentityId = c.Long(nullable: false),
                        IpAddress = c.String(),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IdentityEnt", t => t.IdentityId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.IdentityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Session", "UserId", "dbo.User");
            DropForeignKey("dbo.Session", "IdentityId", "dbo.IdentityEnt");
            DropIndex("dbo.Session", new[] { "IdentityId" });
            DropIndex("dbo.Session", new[] { "UserId" });
            DropTable("dbo.Session");
        }
    }
}
