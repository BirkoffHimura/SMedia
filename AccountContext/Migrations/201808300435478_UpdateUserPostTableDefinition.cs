namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserPostTableDefinition : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserPosts", "User_ID", "dbo.Users");
            DropIndex("dbo.UserPosts", new[] { "User_ID" });
            AlterColumn("dbo.UserPosts", "User_ID", c => c.Long(nullable: false));
            CreateIndex("dbo.UserPosts", "User_ID");
            AddForeignKey("dbo.UserPosts", "User_ID", "dbo.Users", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPosts", "User_ID", "dbo.Users");
            DropIndex("dbo.UserPosts", new[] { "User_ID" });
            AlterColumn("dbo.UserPosts", "User_ID", c => c.Long());
            CreateIndex("dbo.UserPosts", "User_ID");
            AddForeignKey("dbo.UserPosts", "User_ID", "dbo.Users", "ID");
        }
    }
}
