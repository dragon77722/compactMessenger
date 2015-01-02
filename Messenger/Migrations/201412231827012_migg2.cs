namespace Messenger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migg2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.friendPairs",
                c => new
                    {
                        friend1 = c.String(nullable: false, maxLength: 128),
                        friend2 = c.String(),
                    })
                .PrimaryKey(t => t.friend1);
            
            CreateTable(
                "dbo.requestPairs",
                c => new
                    {
                        from = c.String(nullable: false, maxLength: 128),
                        to = c.String(),
                    })
                .PrimaryKey(t => t.from);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.requestPairs");
            DropTable("dbo.friendPairs");
        }
    }
}
