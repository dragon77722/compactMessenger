namespace Messenger.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ming2 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.friendPairs");
            DropPrimaryKey("dbo.requestPairs");
            AddColumn("dbo.friendPairs", "id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.requestPairs", "id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.friendPairs", "friend1", c => c.String());
            AlterColumn("dbo.requestPairs", "from", c => c.String());
            AddPrimaryKey("dbo.friendPairs", "id");
            AddPrimaryKey("dbo.requestPairs", "id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.requestPairs");
            DropPrimaryKey("dbo.friendPairs");
            AlterColumn("dbo.requestPairs", "from", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.friendPairs", "friend1", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.requestPairs", "id");
            DropColumn("dbo.friendPairs", "id");
            AddPrimaryKey("dbo.requestPairs", "from");
            AddPrimaryKey("dbo.friendPairs", "friend1");
        }
    }
}
