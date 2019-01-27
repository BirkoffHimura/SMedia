namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGetUserPicturesByUserIDStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE GetUserPicturesByUserID 
	                @User_ID bigint
                AS
                BEGIN
	                SET NOCOUNT ON;
	    
	                SELECT PictureID, FileName, ProfilePicture, User_ID, UploadDate FROM UserPictures WHERE User_ID = @User_ID
                END
                GO
                ");
        }

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[GetUserPicturesByUserID]");
        }
    }
}
