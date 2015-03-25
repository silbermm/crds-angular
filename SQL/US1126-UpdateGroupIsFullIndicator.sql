USE [MinistryPlatform]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create a placeholder proc, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the proc, as permissions will be maintained
-- on an existing proc.
IF OBJECT_ID('dbo.crds_Update_Group_Is_Full', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.crds_Update_Group_Is_Full AS SELECT 1')
GO
-- =============================================
-- Author:		Jim Kriz
-- Create date: 03/24/2015
-- Description: Update the Group_Is_Full indicator on the dbo.Groups table,
--              using the Target_Size and number of dbo.Group_Participants.
--              This will operate on a single Group if given a Group_ID,
--              or on ALL Groups if Group_Id is <=0.  ("All" groups means
--              all that have no End_Date, or an End_Date >= today).
-- =============================================
ALTER PROCEDURE [dbo].[crds_Update_Group_Is_Full]
	@Group_Id INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Participants_Count INT;
	DECLARE @Target_Size INT;
	DECLARE @Group_Is_Full BIT;

	-- Get a cursor of all the Group_Id values to operate on
	declare @Groups_Cursor CURSOR;
	IF @Group_Id IS NULL
	BEGIN
		-- Look for all groups with no end date, or an end date in the future
		SET @Groups_Cursor = CURSOR FOR
			SELECT DISTINCT Group_Id
			FROM [dbo].[Groups]
			WHERE End_Date IS NULL
			OR CAST(End_Date AS DATE) >= GETDATE()
	END
	ELSE
	BEGIN
		SET @Groups_Cursor = CURSOR FOR
			SELECT @Group_Id
	END

	-- Loop over all impacted groups and update appropriately
	DECLARE @Next_Group_Id INT;
	OPEN @Groups_Cursor
	FETCH NEXT FROM @Groups_Cursor INTO @Next_Group_Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Get the Target_Size from the Group
		SELECT @Target_Size = ISNULL(Target_Size, 0)
		FROM [dbo].[Groups]
		WHERE Group_Id = @Next_Group_Id;

		-- Get the current number of Participants in the group
		SELECT @Participants_Count = COUNT(*)
		FROM [dbo].[Group_Participants]
		WHERE Group_Id = @Next_Group_Id;

		-- Calculate Group_Is_Full
		SET @Group_Is_Full =
			CASE
				-- If the target size is <=0, assuming unlimited, so group is never full
				WHEN @Target_Size <= 0 THEN 0
			 	WHEN @Participants_Count < @Target_Size THEN 0
				ELSE 1
			END;

		-- Set Group_Is_Full
		UPDATE [dbo].[Groups]
		SET Group_Is_Full = @Group_Is_Full
		WHERE Group_Id = @Next_Group_Id;

		FETCH NEXT FROM @Groups_Cursor INTO @Next_Group_Id
	END
	CLOSE @Groups_Cursor
	DEALLOCATE @Groups_Cursor
END
