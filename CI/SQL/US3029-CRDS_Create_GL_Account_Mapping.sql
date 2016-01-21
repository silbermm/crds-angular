USE [MinistryPlatform]
GO

-- Create a placeholder procedure, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the procedure, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.[CRDS_Create_GL_Account_Mapping]', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.[CRDS_Create_GL_Account_Mapping] AS SELECT 1')
GO

-- =============================================
-- Author:      Kocher, Dustin
-- Create date: 01/21/2016
-- Description: Create GL Account Mappings -- This should be removed post go live
-- =============================================
ALTER PROCEDURE [dbo].[CRDS_Create_GL_Account_Mapping]
           @Program_Name nvarchar(200),
           @Congregation_Name nvarchar(200),
           @GL_Account nvarchar(50),
           @Checkbook_ID nvarchar(50),
           @Cash_Account nvarchar(15),
           @Receivable_Account nvarchar(15),
           @Distribution_Account nvarchar(50),
           @Document_Type nvarchar(50),
           @Customer_ID nvarchar(50),
           @Scholarship_Expense_Account nvarchar(15)
AS
BEGIN
	-- Get the MP Congregation_ID
	DECLARE @Congregation_ID Int;
	SELECT @Congregation_ID = [Congregation_ID] FROM [dbo].[Congregations] WHERE [Congregation_Name] = RTRIM(LTRIM(@Congregation_Name));

	IF @Congregation_ID IS NULL
	BEGIN
		PRINT @Congregation_Name + ',' + @Program_Name + ',' + @GL_Account + ',CongregationNotFound'
		RETURN
	END;

	-- Get the MP Program_ID
	DECLARE @Program_ID Int;
	SELECT @Program_ID = [Program_ID] FROM [dbo].[Programs] WHERE [Program_Name] = RTRIM(LTRIM(@Program_Name));
	
	IF @Program_ID IS NULL
	BEGIN
		PRINT @Congregation_Name + ',' + @Program_Name + ',' + @GL_Account + ',ProgramNotFound'
		RETURN
	END;

	-- Only insert if it isn't already there, in case we have to run this more than once
	BEGIN TRY
		IF NOT EXISTS (SELECT 1  FROM [dbo].[GL_Account_Mapping] WHERE [Domain_ID] = 1 AND [Program_ID] = @Program_ID AND [Congregation_ID] = @Congregation_ID AND [GL_Account] = @GL_Account)
			BEGIN
				INSERT INTO [dbo].[GL_Account_Mapping]
					   ([Domain_ID]
					   ,[Program_ID]
					   ,[Congregation_ID]
					   ,[GL_Account]
					   ,[Checkbook_ID]
					   ,[Cash_Account]
					   ,[Receivable_Account]
					   ,[Distribution_Account]
					   ,[Document_Type]
					   ,[Customer_ID]
					   ,[Scholarship_Expense_Account])
				 VALUES
					   (1, 
					   @Program_ID, 
					   @Congregation_ID,
					   @GL_Account,
					   @Checkbook_ID,
					   @Cash_Account,
					   @Receivable_Account,
					   @Distribution_Account,
					   @Document_Type,
					   @Customer_ID,
					   @Scholarship_Expense_Account)
				PRINT @Congregation_Name + ',' + @Program_Name + ',' + @GL_Account + ',Added';
			END;
		ELSE
			BEGIN
				PRINT @Congregation_Name + ',' + @Program_Name + ',' + @GL_Account + ',Skipped';
			END;
	END TRY
	BEGIN CATCH
		PRINT @Congregation_Name + ',' + @Program_Name + ',' + @GL_Account + ',Error: ' + ERROR_MESSAGE();
	END CATCH
END;
GO
