USE [MinistryPlatform]
GO

ALTER TABLE dbo.Attributes ALTER COLUMN [Attribute_Name] nvarchar(100) not null
GO

-- ATTRIBUTE TYPE DETAILS
DECLARE @ATTRIBUTE_TYPE_ID int = 67;
DECLARE @ATTRIBUTE_TYPE_VALUE varchar(50) = N'Allergies';
DECLARE @ATTRIBUTE_TYPE_DESCRIPTION varchar(255) = N'Allergies';

DECLARE @ATTRIBUTE_ID_1 int = 3971;
DECLARE @ATTRIBUTE_NAME_1 varchar(100) = N'All Allergies';

IF EXISTS (Select 1 FROM [dbo].[Attribute_Types] WHERE [Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID)
	BEGIN
		UPDATE [dbo].[Attribute_Types]
			SET [Attribute_Type] = @ATTRIBUTE_TYPE_VALUE
			   ,[Description] = @ATTRIBUTE_TYPE_DESCRIPTION
			   ,[Domain_ID] = 1
			   ,[Available_Online] = 1
			WHERE [dbo].Attribute_Types.Attribute_Type_ID = @ATTRIBUTE_TYPE_ID
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Attribute_Types] ON
		INSERT INTO [dbo].[Attribute_Types]
				   ( [Attribute_Type_ID]
				   ,[Attribute_Type]
				   ,[Description]
				   ,[Domain_ID]
				   ,[Available_Online])
			 VALUES
				   (@ATTRIBUTE_TYPE_ID
				   ,@ATTRIBUTE_TYPE_VALUE
				   ,@ATTRIBUTE_TYPE_DESCRIPTION
				   ,1
				   ,1)
			SET IDENTITY_INSERT [dbo].[Attribute_Types] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_1)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_1
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_1
	END
ELSE
	BEGIN
		SET IDENTITY_INSERT [dbo].[Attributes] ON
		INSERT INTO [dbo].[Attributes]
				   ( [Attribute_ID]
				   ,[Attribute_Name]
				   ,[Domain_ID]
				   ,[Attribute_Type_ID])
			 VALUES
				   (@ATTRIBUTE_ID_1
				   ,@ATTRIBUTE_NAME_1
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END