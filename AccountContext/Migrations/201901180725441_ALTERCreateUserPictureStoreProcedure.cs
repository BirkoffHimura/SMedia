namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALTERCreateUserPictureStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                ALTER PROCEDURE [dbo].[CreateUserPicture] 
	                @FileName nvarchar(60),
	                @ProfilePicture bit,
	                @User_ID bigint,
	                @UploadDate datetime,
					@PictureID bigint OUTPUT
                AS
                BEGIN
	                SET NOCOUNT ON;

	                INSERT INTO UserPictures(FileName, ProfilePicture, User_ID, UploadDate) VALUES(@FileName, @ProfilePicture, @User_ID, @UploadDate)

	                SET @PictureID = @@IDENTITY
                END
                ");
        }
        
        public override void Down()
        {
            Sql(@"
                ALTER PROCEDURE [dbo].[CreateUserPicture] 
	                @FileName nvarchar(60),
	                @ProfilePicture bit,
	                @User_ID bigint,
	                @UploadDate datetime
                AS
                BEGIN
	                SET NOCOUNT ON;

	                INSERT INTO UserPictures(FileName, ProfilePicture, User_ID, UploadDate) VALUES(@FileName, @ProfilePicture, @User_ID, @UploadDate)

	                SELECT @@IDENTITY as 'PictureID'
                END
                GO
                ");
        }
    }
}
