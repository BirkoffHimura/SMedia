namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSmallColumnSizesOnSeveralTables : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.UserPictures", "FileName", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.UserPosts", "Subject", c => c.String(maxLength: 60));
            CreateIndex("dbo.Users", "UserName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            AlterColumn("dbo.UserPosts", "Subject", c => c.String(maxLength: 25));
            AlterColumn("dbo.UserPictures", "FileName", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Users", "UserName", unique: true);
        }
    }
}
