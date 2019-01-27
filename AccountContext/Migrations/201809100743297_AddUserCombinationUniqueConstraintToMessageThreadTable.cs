namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserCombinationUniqueConstraintToMessageThreadTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MessageThreads", "UserCombination", c => c.String(nullable: false, maxLength: 41));
            CreateIndex("dbo.MessageThreads", "UserCombination", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.MessageThreads", new[] { "UserCombination" });
            AlterColumn("dbo.MessageThreads", "UserCombination", c => c.String(maxLength: 41));
        }
    }
}
