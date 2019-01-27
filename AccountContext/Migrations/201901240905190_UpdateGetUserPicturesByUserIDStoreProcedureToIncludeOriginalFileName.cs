namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGetUserPicturesByUserIDStoreProcedureToIncludeOriginalFileName : DbMigration
    {
        public override void Up()
        {
            Sql(@"
ALTER PROCEDURE [dbo].[GetUserPicturesByUserID] 
    @User_ID bigint
AS
BEGIN
    SET NOCOUNT ON;
	    
    SELECT PictureID, FileName, OriginalFileName, ProfilePicture, User_ID, UploadDate FROM UserPictures WHERE User_ID = @User_ID
END
            ");
        }
        
        public override void Down()
        {
            Sql(@"
ALTER PROCEDURE [dbo].[GetUserPicturesByUserID] 
    @User_ID bigint
AS
BEGIN
    SET NOCOUNT ON;
	    
    SELECT PictureID, FileName, ProfilePicture, User_ID, UploadDate FROM UserPictures WHERE User_ID = @User_ID
END
            ");
        }
    }
}
