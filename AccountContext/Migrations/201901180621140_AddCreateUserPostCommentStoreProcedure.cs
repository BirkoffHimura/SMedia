namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateUserPostCommentStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE CreateUserPostComment 
	                @Comment_Body nvarchar(256),
	                @CommentDate datetime,
	                @FromUser_ID bigint,
	                @UserPost_ID bigint
                AS
                BEGIN
	                SET NOCOUNT ON;

	                INSERT INTO UserPostComments(Comment_Body, CommentDate, FromUser_ID, UserPost_ID) VALUES(@Comment_Body, @CommentDate, @FromUser_ID, @UserPost_ID)

	                SELECT @@IDENTITY as 'PostCommentID'
                END
                GO
                ");
        }
        
        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[CreateUserPostComment]");
        }
    }
}
