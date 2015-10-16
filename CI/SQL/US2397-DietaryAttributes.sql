USE [MinistryPlatform]
GO

ALTER TABLE dbo.Attributes ALTER COLUMN [Attribute_Name] nvarchar(100) not null
GO

-- ATTRIBUTE TYPE DETAILS
DECLARE @ATTRIBUTE_TYPE_ID int = 65;
DECLARE @ATTRIBUTE_TYPE_VALUE varchar(50) = N'Dietary Restrictions';
DECLARE @ATTRIBUTE_TYPE_DESCRIPTION varchar(255) = N'Record dietary restrictions and allergies';

DECLARE @ATTRIBUTE_ID_VEGETARIAN int = 3961;
DECLARE @ATTRIBUTE_NAME_VEGETARIAN varchar(100) = N'Vegetarian';

DECLARE @ATTRIBUTE_ID_VEGAN int = 3962;
DECLARE @ATTRIBUTE_NAME_VEGAN varchar(100) = N'Vegan';

DECLARE @ATTRIBUTE_ID_NOGLUTEN int = 3963;
DECLARE @ATTRIBUTE_NAME_NOGLUTEN varchar(100) = N'No Gluten';

DECLARE @ATTRIBUTE_ID_NOEGG int = 3964;
DECLARE @ATTRIBUTE_NAME_NOEGG varchar(100) = N'No Egg';

DECLARE @ATTRIBUTE_ID_NODAIRY int = 3965;
DECLARE @ATTRIBUTE_NAME_NODAIRY varchar(100) = N'No Dairy';

DECLARE @ATTRIBUTE_ID_NOFISH int = 3966;
DECLARE @ATTRIBUTE_NAME_NOFISH varchar(100) = N'No Fish';

DECLARE @ATTRIBUTE_ID_NONUTS int = 3967;
DECLARE @ATTRIBUTE_NAME_NONUTS varchar(100) = N'No Nuts';

DECLARE @ATTRIBUTE_ID_OTHER int = 3963;
DECLARE @ATTRIBUTE_NAME_OTHER varchar(100) = N'Other';

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

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_VEGETARIAN)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_VEGETARIAN
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_VEGETARIAN
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
				   (@ATTRIBUTE_ID_VEGETARIAN
				   ,@ATTRIBUTE_NAME_VEGETARIAN
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END


IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_VEGAN)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_VEGAN
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_VEGAN
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
				   (@ATTRIBUTE_ID_VEGAN
				   ,@ATTRIBUTE_NAME_VEGAN
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_NOGLUTEN)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_NOGLUTEN
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_NOGLUTEN
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
				   (@ATTRIBUTE_ID_NOGLUTEN
				   ,@ATTRIBUTE_NAME_NOGLUTEN
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

	IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_NODAIRY)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_NODAIRY
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_NODAIRY
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
				   (@ATTRIBUTE_ID_NODAIRY
				   ,@ATTRIBUTE_NAME_NODAIRY
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_NOEGG)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_NOEGG
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_NOEGG
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
				   (@ATTRIBUTE_ID_NOEGG
				   ,@ATTRIBUTE_NAME_NOEGG
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_NOFISH)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_NOFISH
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_NOFISH
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
				   (@ATTRIBUTE_ID_NOFISH
				   ,@ATTRIBUTE_NAME_NOFISH
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_NONUTS)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_NONUTS
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_NONUTS
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
				   (@ATTRIBUTE_ID_NONUTS
				   ,@ATTRIBUTE_NAME_NONUTS
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_OTHER)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_OTHER
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_OTHER
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
				   (@ATTRIBUTE_ID_OTHER
				   ,@ATTRIBUTE_NAME_OTHER
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END
