namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreatePostStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    CREATE PROCEDURE CreatePost 
                    @Subject nvarchar(60),
                    @Post_Body nvarchar(256),
                    @Img_Extern nvarchar(max) = null,
                    @UserPicture_PictureID bigint = null,
                    @User_ID bigint,
                    @PostDate datetime,
                    @ID bigint output
                    AS
                    BEGIN
                        SET NOCOUNT ON;
                        IF @Img_Extern IS NULL AND @UserPicture_PictureID IS NULL
						BEGIN
							INSERT INTO UserPosts(Subject, Post_Body, User_ID, PostDate) VALUES(@Subject, @Post_Body, @User_ID, @PostDate)
						END
						ELSE IF @Img_Extern IS NULL
						BEGIN
							INSERT INTO UserPosts(Subject, Post_Body, UserPicture_PictureID, User_ID, PostDate) VALUES(@Subject, @Post_Body, @UserPicture_PictureID, @User_ID, @PostDate)
						END
						ELSE IF @UserPicture_PictureID IS NULL
						BEGIN
							INSERT INTO UserPosts(Subject, Post_Body, Img_Extern, User_ID, PostDate) VALUES(@Subject, @Post_Body, @Img_Extern, @User_ID, @PostDate)
						END
						ELSE
						BEGIN
							INSERT INTO UserPosts(Subject, Post_Body, Img_Extern, UserPicture_PictureID, User_ID, PostDate) VALUES(@Subject, @Post_Body, @Img_Extern, @UserPicture_PictureID, @User_ID, @PostDate)
						END

                        set @ID = @@IDENTITY
                        IF @ID > 0
                        BEGIN
                            RETURN 1
                        END
                        ELSE
                        BEGIN
                            RETURN -1
                        END	
                    END
                    GO
");

        }

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[CreatePost]");
        }
    }
}
