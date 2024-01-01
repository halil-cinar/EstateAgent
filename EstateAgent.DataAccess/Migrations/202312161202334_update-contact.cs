namespace EstateAgent.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecontact : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContactInfo", "XUrl", c => c.String());
            AddColumn("dbo.ContactInfo", "InstagramUrl", c => c.String());
            AddColumn("dbo.ContactInfo", "FacebookUrl", c => c.String());
            AddColumn("dbo.ContactInfo", "LinkedinUrl", c => c.String());
            AddColumn("dbo.ContactInfo", "LocationLongitude", c => c.String());
            AddColumn("dbo.ContactInfo", "LocationLatitude", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContactInfo", "LocationLatitude");
            DropColumn("dbo.ContactInfo", "LocationLongitude");
            DropColumn("dbo.ContactInfo", "LinkedinUrl");
            DropColumn("dbo.ContactInfo", "FacebookUrl");
            DropColumn("dbo.ContactInfo", "InstagramUrl");
            DropColumn("dbo.ContactInfo", "XUrl");
        }
    }
}
