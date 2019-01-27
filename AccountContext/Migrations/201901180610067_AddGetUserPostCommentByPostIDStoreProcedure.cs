namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetUserPostCommentByPostIDStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetUserPostCommentByPostID 
	                @UserPost_ID bigint	
                AS
                BEGIN
	                -- SET NOCOUNT ON added to prevent extra result sets from
	                -- interfering with SELECT statements.
	                SET NOCOUNT ON;

                    -- Insert statements for procedure here
	                SELECT PostCommentID, Comment_Body, CommentDate, FromUser_ID, UserPost_ID FROM UserPostComments WHERE UserPost_ID = @UserPost_ID
                END
                GO                    
            ");

        }

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[GetUserPostCommentByPostID]");
        }
    }
}
