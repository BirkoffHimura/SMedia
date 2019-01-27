namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAfterUserMessageTableDefinitionAddition : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.UserMessages");
            AlterColumn("dbo.UserMessages", "UserMessageID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.UserMessages", "UserMessageID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.UserMessages");
            AlterColumn("dbo.UserMessages", "UserMessageID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UserMessages", "UserMessageID");
        }
    }
}
