USE [MinistryPlatform]
GO

DECLARE @RELATIONSHIP_SPONSORED_CHILD_ID int = 43;
DECLARE @RELATIONSHIP_SPONSORED_CHILD_NAME varchar(100) = N'Sponsored child';

DECLARE @RELATIONSHIP_SPONSORED_CHILD_OF_ID int = 44;
DECLARE @RELATIONSHIP_SPONSORED_CHILD_OF_NAME varchar(100) = N'Sponsored child of';

IF EXISTS (Select 1 FROM [dbo].[Relationships] WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_ID)
	BEGIN
		UPDATE [dbo].[Relationships] 
		SET [Relationship_Name] = @RELATIONSHIP_SPONSORED_CHILD_NAME
		WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_ID
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Relationships] ON
		INSERT INTO [dbo].[Relationships]
			   ([Relationship_ID]
			   ,[Relationship_Name]
			   ,[ImmediateFamily]
			   ,[Age_Check])
		 VALUES
			   (@RELATIONSHIP_SPONSORED_CHILD_ID
			   ,@RELATIONSHIP_SPONSORED_CHILD_NAME,
			   0,
			   0)
		SET IDENTITY_INSERT [dbo].[Relationships] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Relationships] WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_OF_ID)
	BEGIN
		UPDATE [dbo].[Relationships] 
		SET [Relationship_Name] = @RELATIONSHIP_SPONSORED_CHILD_OF_NAME
		WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_OF_ID
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Relationships] ON
		INSERT INTO [dbo].[Relationships]
			   ([Relationship_ID]
			   ,[Relationship_Name]
			   ,[ImmediateFamily]
			   ,[Age_Check])
		 VALUES
			   (@RELATIONSHIP_SPONSORED_CHILD_OF_ID
			   ,@RELATIONSHIP_SPONSORED_CHILD_OF_NAME,
			   0,
			   0)
		SET IDENTITY_INSERT [dbo].[Relationships] OFF
	END

UPDATE [dbo].[Relationships] 
SET [Reciprocal_Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_ID
WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_OF_ID

UPDATE [dbo].[Relationships]
SET [Reciprocal_Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_OF_ID
WHERE [Relationship_ID] = @RELATIONSHIP_SPONSORED_CHILD_ID
