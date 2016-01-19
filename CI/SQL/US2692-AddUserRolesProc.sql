USE [MinistryPlatform]
GO

-- Create a placeholder procedure, so the below ALTER will always work.
-- This allows the same SQL file to be run in any environment, without errors.
-- This is preferable to dropping the procedure, as permissions will be maintained
-- on an existing function.
IF OBJECT_ID('dbo.CRDS_Add_User_Roles', 'p') IS NULL
    EXEC('CREATE PROCEDURE dbo.CRDS_Add_User_Roles AS SELECT 1')
GO


-- =============================================
-- Author:      Kriz, Jim
-- Create date: 01/19/2016
-- Description: Add User Roles for a given Legacy Person ID
-- =============================================
ALTER PROCEDURE [dbo].[CRDS_Add_User_Roles]
         @LegacyPersonID Int
        ,@RoleIDs Varchar(4000)
AS
BEGIN
	-- Split the comma-separated list of Role IDs into a table of int Role IDs
	DECLARE @Roles TABLE(Ix Int identity(1,1), RoleID Int);
	INSERT INTO @Roles(RoleID) SELECT CONVERT(INT, Item) FROM dp_Split(@RoleIDs, ',');

	-- Get the MP User_ID for the given Legacy Person ID
	DECLARE @UserID Int;
	SELECT @UserID = [User_ID] FROM [dbo].[dp_Users] WHERE [__ExternalPersonID] = @LegacyPersonID;

	DECLARE @i Int;
	DECLARE @count Int;

	-- Loop over Role IDs, insert one dp_User_Role for each
	SELECT @i = min(Ix) - 1, @count = max(Ix) FROM @Roles;
	DECLARE @RoleID Int;
	WHILE @i < @count
	BEGIN
		SELECT @i = @i + 1;
		SELECT @RoleID = RoleID from @Roles WHERE Ix = @i;

		BEGIN TRY
			IF @UserID IS NULL
			BEGIN
				PRINT CONVERT(varchar, @LegacyPersonID) + ',NULL,' + CONVERT(varchar, @RoleID) + ',UserNotFound';
				CONTINUE
			END;

			-- Only insert if it isn't already there, in case we have to run this more than once
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
END;
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
-- Description: Add User Role for a given Legacy Person ID
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