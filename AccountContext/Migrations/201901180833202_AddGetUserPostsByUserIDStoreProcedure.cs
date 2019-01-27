namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetUserPostsByUserIDStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetUserPostsByUserID
	                @User_ID bigint
                AS
                BEGIN
	                SET NOCOUNT ON;

	                SELECT ID, Subject, Post_Body, Img_Extern, UserPicture_PictureID, User_ID, PostDate FROM UserPosts WHERE User_ID = @User_ID
                END
                GO
                ");
        }

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[GetUserPostsByUserID]");
        }
    }
}
