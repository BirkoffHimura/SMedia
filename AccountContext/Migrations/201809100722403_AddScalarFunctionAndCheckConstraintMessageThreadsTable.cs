namespace AccountContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScalarFunctionAndCheckConstraintMessageThreadsTable : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                    CREATE FUNCTION CheckUserCombination 
                    (
	                    @TheText nvarchar(41)
                    )
                    RETURNS bit
                    AS
                    BEGIN
	                    DECLARE @TheDelimiter nvarchar(1);
	                    DECLARE @Num1 bigint;
	                    DECLARE @Num2 bigint;
	                    DECLARE @counter int;
	                    DECLARE @length int;
	                    DECLARE @NumOneCompleted bit;
	                    DECLARE @TempStr nvarchar(20);
	 
	                    set @TheDelimiter = ':';
	                    set @TempStr = '';

	                    set @length = len(@TheText);
	                    set @counter = 1;
	                    set @NumOneCompleted = 0;

	                    while @counter <= @length
	                    begin
		                    IF SUBSTRING(@TheText, @counter, 1) = ':'
		                    BEGIN
			                    set @Num1 = CAST(@TempStr as bigint);
			                    set @NumOneCompleted = 1;
			                    set @counter = @counter + 1;
			                    set @TempStr = '';
		                    END
		                    ELSE
		                    BEGIN
			                    set @TempStr = @TempStr + SUBSTRING(@TheText, @counter, 1);
			                    set @counter = @counter + 1;
		                    END
	                    end
	                    set @Num2 = CAST(@TempStr as bigint);

	                    IF @Num1 < @Num2
	                    BEGIN
		                    RETURN 1;
	                    END
	                    ELSE
	                    BEGIN
		                    RETURN 0;
	                    END
	                    RETURN 0;
                    END
                    GO
                    
                    ALTER TABLE MessageThreads
                    ADD CONSTRAINT CHK_UserCombination CHECK (dbo.CheckUserCombination(UserCombination) > 0);
                ");
        }
        
        public override void Down()
        {
            Sql(@"DROP FUNCTION dbo.CheckUserCombination;
                ALTER TABLE MessageThreads
                DROP CHECK CHK_UserCombination;
            ");
        }
    }
}
