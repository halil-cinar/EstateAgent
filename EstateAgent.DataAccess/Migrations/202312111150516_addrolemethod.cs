namespace EstateAgent.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addrolemethod : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleMethodEntities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Role = c.Int(nullable: false),
                        Method = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsDeletable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RoleMethodEntities");
        }
    }
}
