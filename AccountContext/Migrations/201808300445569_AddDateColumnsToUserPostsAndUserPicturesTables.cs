namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateColumnsToUserPostsAndUserPicturesTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserPictures", "UploadDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserPosts", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserPosts", "PostDate");
            DropColumn("dbo.UserPictures", "UploadDate");
        }
    }
}
