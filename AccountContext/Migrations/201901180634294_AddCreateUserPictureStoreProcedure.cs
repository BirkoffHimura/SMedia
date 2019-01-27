namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateUserPictureStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE PROCEDURE CreateUserPicture 
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

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[CreateUserPicture]");
        }
    }
}
