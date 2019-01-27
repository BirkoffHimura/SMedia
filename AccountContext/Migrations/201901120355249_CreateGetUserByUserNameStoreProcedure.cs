namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateGetUserByUserNameStoreProcedure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    CREATE PROCEDURE GetUserByUserName 
                    @UserName nvarchar(50)
                    AS
                    BEGIN
                    SET NOCOUNT ON;
	    
                    SELECT ID, Name, UserName, Password, AddressLine1, AddressLine2, Country, City, State, ZipCode, BirthDate, SignupDate, SmallBio FROM Users WHERE UserName = @UserName
                    END
                    GO");

        }

        public override void Down()
        {
            Sql("DROP PROCEDURE [dbo].[GetUserByUserName]");
        }
    }
}
