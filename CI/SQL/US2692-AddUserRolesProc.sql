USE [MinistryPlatform]
GO

-- Create a placeholder procedure, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the procedure, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.CRDS_Add_User_Role', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.CRDS_Add_User_Role AS SELECT 1')
GO

-- =============================================
-- Author:      Kriz, Jim
-- Create date: 01/19/2016
-- Description: Add User Role for a given Legacy Person ID.  This procedure
--              is only needed during cutover from Legacy to MinistryPlatform.
--              It can be safely deleted once we have migrated users and roles.
--              There is a story in Rally to remove this, US3083 (https://rally1.rallydev.com/#/27593764268d/detail/userstory/49993426884).
-- =============================================
ALTER PROCEDURE [dbo].[CRDS_Add_User_Role]
         @LegacyPersonID Int
        ,@RoleID Int
AS
BEGIN
	-- Get the MP User_ID for the given Legacy Person ID
	DECLARE @UserID Int;
	SELECT @UserID = [User_ID] FROM [dbo].[dp_Users] WHERE [__ExternalPersonID] = @LegacyPersonID;
	IF @UserID IS NULL
	BEGIN
		PRINT CONVERT(varchar, @LegacyPersonID) + ',NULL,' + CONVERT(varchar, @RoleID) + ',UserNotFound';
		RETURN
	END;

	-- Only insert if it isn't already there, in case we have to run this more than once
	BEGIN TRY
		IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_User_Roles] WHERE User_ID = @UserID AND Role_ID = @RoleID AND Domain_ID = 1)
			BEGIN
				INSERT INTO [dbo].[dp_User_Roles] (User_ID, Role_ID, Domain_ID) VALUES (@UserID, @RoleID, 1);
				PRINT CONVERT(varchar, @LegacyPersonID) + ',' + CONVERT(varchar, @UserID) + ',' + CONVERT(varchar, @RoleID) + ',Added';
			END;
		ELSE
			BEGIN
				PRINT CONVERT(varchar, @LegacyPersonID) + ',' + CONVERT(varchar, @UserID) + ',' + CONVERT(varchar, @RoleID) + ',Skipped';
			END;
	END TRY
	BEGIN CATCH
		PRINT CONVERT(varchar, @LegacyPersonID) + ',' + CONVERT(varchar, @UserID) + ',' + CONVERT(varchar, @RoleID) + ',Error: ' + ERROR_MESSAGE();
	END CATCH
END;
GO