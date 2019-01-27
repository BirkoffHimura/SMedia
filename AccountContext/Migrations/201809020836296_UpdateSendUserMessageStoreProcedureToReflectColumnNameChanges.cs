namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSendUserMessageStoreProcedureToReflectColumnNameChanges : DbMigration
    {
        public override void Up()
        {
            Sql(@"
            ALTER PROCEDURE [dbo].[SendUserMessage]
	            @SenderID bigint, 
	            @ReceiverID bigint,
	            @MessageText nvarchar(500),
                @MessageID bigint OUTPUT
            AS
            BEGIN

	            SET NOCOUNT ON;
	            DECLARE @UserCombination nvarchar(41); 
	            DECLARE @MessageThreadID bigint;

	            If @SenderID = @ReceiverID
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END

	            If (SELECT COUNT(*) FROM Users WHERE ID = @SenderID) < 1
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END

	            If (SELECT COUNT(*) FROM Users WHERE ID = @ReceiverID) < 1
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END
	
	            If @SenderID < @ReceiverID
	            BEGIN
		            set @UserCombination = CAST(@SenderID as nvarchar) + ':' + CAST(@ReceiverID as nvarchar);
	            END
	            ELSE
	            BEGIN
		            set @UserCombination = CAST(@ReceiverID as nvarchar) + ':' + CAST(@SenderID as nvarchar);
	            END
	
	            SELECT TOP 1 @MessageThreadID = MessageThreadID 
	            FROM MessageThreads 
	            WHERE UserCombination = @UserCombination;

	            IF @MessageThreadID IS NULL
	            BEGIN
		            INSERT INTO MessageThreads(UserCombination) VALUES(@UserCombination);
		            SET @MessageThreadID = @@IDENTITY;
	            END
    
	            INSERT INTO UserMessages(MessageThreadID, UserID, MessageBody) VALUES(@MessageThreadID, @SenderID, @MessageText);
                SET @MessageID = @@IDENTITY;
                SELECT @MessageID;
	            RETURN @MessageID;	
                END");
        }
        
        public override void Down()
        {
            Sql(@"
            ALTER PROCEDURE [dbo].[SendUserMessage]
	            @SenderID bigint, 
	            @ReceiverID bigint,
	            @MessageText nvarchar(500),
                @MessageID bigint OUTPUT
            AS
            BEGIN

	            SET NOCOUNT ON;
	            DECLARE @UserCombination nvarchar(41); 
	            DECLARE @MessageThreadID bigint;

	            If @SenderID = @ReceiverID
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END

	            If (SELECT COUNT(*) FROM Users WHERE ID = @SenderID) < 1
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END

	            If (SELECT COUNT(*) FROM Users WHERE ID = @ReceiverID) < 1
	            BEGIN
                    SET @MessageID = -1;
		            RETURN -1;
	            END
	
	            If @SenderID < @ReceiverID
	            BEGIN
		            set @UserCombination = CAST(@SenderID as nvarchar) + ':' + CAST(@ReceiverID as nvarchar);
	            END
	            ELSE
	            BEGIN
		            set @UserCombination = CAST(@ReceiverID as nvarchar) + ':' + CAST(@SenderID as nvarchar);
	            END
	
	            SELECT TOP 1 @MessageThreadID = MessageThreadID 
	            FROM MessageThreads 
	            WHERE UserCombination = @UserCombination;

	            IF @MessageThreadID IS NULL
	            BEGIN
		            INSERT INTO MessageThreads(UserCombination) VALUES(@UserCombination);
		            SET @MessageThreadID = @@IDENTITY;
	            END
    
	            INSERT INTO UserMessages(MessageThread_MessageThreadID, User_ID, MessageBody) VALUES(@MessageThreadID, @SenderID, @MessageText);
                SET @MessageID = @@IDENTITY;
                SELECT @MessageID;
	            RETURN @MessageID;	
                END");
        }
    }
}
