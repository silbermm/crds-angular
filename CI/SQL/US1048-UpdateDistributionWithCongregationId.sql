USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.crds_Update_Distribution_With_Congregation_Id', 'p') IS NULL
	EXEC('CREATE PROCEDURE dbo.crds_Update_Distribution_With_Congregation_Id AS SELECT 1')
GO

-- =============================================
-- Author:		Matt Brewer
-- Create date: 6/10/2015
-- Description:	Refresh Congregation_Ids on Distributions after data migrations
-- =============================================
ALTER PROCEDURE [dbo].[crds_Update_Distribution_With_Congregation_Id]
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Distribution_Cursor CURSOR;

	BEGIN
		SET @Distribution_Cursor = CURSOR FOR
			SELECT h.congregation_id
				FROM [dbo].[Donation_Distributions] dd
				JOIN [dbo].[Donations] d ON d.donation_id = dd.donation_id
				JOIN [dbo].[Donors] do ON do.donor_id = d.donor_id
				JOIN [dbo].[Contacts] c ON c.contact_id = do.contact_id
				JOIN [dbo].[Households] h ON h.household_id = c.household_id
				WHERE dd.Congregation_ID IS NULL
	END

	OPEN @Distribution_Cursor
	DECLARE @Household_Congregation_Id INT;
	FETCH NEXT FROM @Distribution_Cursor INTO @Household_Congregation_Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Set to 'No Site Specified' if Household has no Congregation		
		IF @Household_Congregation_Id IS NULL
			SET @Household_Congregation_Id = 5;

		UPDATE [dbo].[Donation_Distributions]
		SET Congregation_ID = @Household_Congregation_Id
		WHERE CURRENT OF @Distribution_Cursor;

		FETCH NEXT FROM @Distribution_Cursor INTO @Household_Congregation_Id
	END
	CLOSE @Distribution_Cursor;
	DEALLOCATE @Distribution_Cursor;
		
END