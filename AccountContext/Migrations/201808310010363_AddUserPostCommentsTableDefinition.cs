namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPostCommentsTableDefinition : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserPostComments",
                c => new
                    {
                        PostCommentID = c.Long(nullable: false, identity: true),
                        Comment_Body = c.String(nullable: false, maxLength: 256),
                        CommentDate = c.DateTime(nullable: false),
                        FromUser_ID = c.Long(nullable: false),
                        UserPost_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.PostCommentID)
                .ForeignKey("dbo.Users", t => t.FromUser_ID, cascadeDelete: false)
                .ForeignKey("dbo.UserPosts", t => t.UserPost_ID, cascadeDelete: true)
                .Index(t => t.FromUser_ID)
                .Index(t => t.UserPost_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserPostComments", "UserPost_ID", "dbo.UserPosts");
            DropForeignKey("dbo.UserPostComments", "FromUser_ID", "dbo.Users");
            DropIndex("dbo.UserPostComments", new[] { "UserPost_ID" });
            DropIndex("dbo.UserPostComments", new[] { "FromUser_ID" });
            DropTable("dbo.UserPostComments");
        }
    }
}
