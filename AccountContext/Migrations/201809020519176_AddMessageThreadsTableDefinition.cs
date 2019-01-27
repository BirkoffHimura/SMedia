namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageThreadsTableDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageThreads",
                c => new
                    {
                        MessageThreadID = c.Long(nullable: false, identity: true),
                        UserCombination = c.String(maxLength: 41),
                    })
                .PrimaryKey(t => t.MessageThreadID);            
        }
        
        public override void Down()
        {
            DropTable("dbo.MessageThreads");            
        }
    }
}
