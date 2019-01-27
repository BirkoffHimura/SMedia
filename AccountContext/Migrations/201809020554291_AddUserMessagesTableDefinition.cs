namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserMessagesTableDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserMessages",
                c => new
                    {
                        UserMessageID = c.Long(nullable: false, identity: true),
                        MessageBody = c.String(nullable: false, maxLength: 500),
                        MessageThread_MessageThreadID = c.Long(),
                        User_ID = c.Long(),
                    })
                .PrimaryKey(t => t.UserMessageID)
                .ForeignKey("dbo.MessageThreads", t => t.MessageThread_MessageThreadID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.MessageThread_MessageThreadID)
                .Index(t => t.User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMessages", "User_ID", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "MessageThread_MessageThreadID", "dbo.MessageThreads");
            DropIndex("dbo.UserMessages", new[] { "User_ID" });
            DropIndex("dbo.UserMessages", new[] { "MessageThread_MessageThreadID" });
            DropTable("dbo.UserMessages");
        }
    }
}
