namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateForeignKeysForUserMessagesMessageThreadsAndUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserMessages", "MessageThread_MessageThreadID", "dbo.MessageThreads");
            DropForeignKey("dbo.UserMessages", "User_ID", "dbo.Users");
            DropIndex("dbo.UserMessages", new[] { "MessageThread_MessageThreadID" });
            DropIndex("dbo.UserMessages", new[] { "User_ID" });
            RenameColumn(table: "dbo.UserMessages", name: "MessageThread_MessageThreadID", newName: "MessageThreadID");
            RenameColumn(table: "dbo.UserMessages", name: "User_ID", newName: "UserID");
            AlterColumn("dbo.UserMessages", "MessageThreadID", c => c.Long(nullable: false));
            AlterColumn("dbo.UserMessages", "UserID", c => c.Long(nullable: false));
            CreateIndex("dbo.UserMessages", "MessageThreadID");
            CreateIndex("dbo.UserMessages", "UserID");
            AddForeignKey("dbo.UserMessages", "MessageThreadID", "dbo.MessageThreads", "MessageThreadID", cascadeDelete: true);
            AddForeignKey("dbo.UserMessages", "UserID", "dbo.Users", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserMessages", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserMessages", "MessageThreadID", "dbo.MessageThreads");
            DropIndex("dbo.UserMessages", new[] { "UserID" });
            DropIndex("dbo.UserMessages", new[] { "MessageThreadID" });
            AlterColumn("dbo.UserMessages", "UserID", c => c.Long());
            AlterColumn("dbo.UserMessages", "MessageThreadID", c => c.Long());
            RenameColumn(table: "dbo.UserMessages", name: "UserID", newName: "User_ID");
            RenameColumn(table: "dbo.UserMessages", name: "MessageThreadID", newName: "MessageThread_MessageThreadID");
            CreateIndex("dbo.UserMessages", "User_ID");
            CreateIndex("dbo.UserMessages", "MessageThread_MessageThreadID");
            AddForeignKey("dbo.UserMessages", "User_ID", "dbo.Users", "ID");
            AddForeignKey("dbo.UserMessages", "MessageThread_MessageThreadID", "dbo.MessageThreads", "MessageThreadID");
        }
    }
}
