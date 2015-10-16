USE [MinistryPlatform]
GO

ALTER TABLE dbo.Attributes ALTER COLUMN [Attribute_Name] nvarchar(100) not null
GO

-- ATTRIBUTE TYPE DETAILS
DECLARE @ATTRIBUTE_TYPE_ID int = 60;
DECLARE @ATTRIBUTE_TYPE_VALUE varchar(50) = N'Define your Spiritual Life';
DECLARE @ATTRIBUTE_TYPE_DESCRIPTION varchar(255) = N'Where someone is on their journey with God';

DECLARE @ATTRIBUTE_ID_SEARCING int = 3933;
DECLARE @ATTRIBUTE_NAME_SEARCHING varchar(100) = N'Searching for answers';

DECLARE @ATTRIBUTE_ID_RECIEVED int = 3934;
DECLARE @ATTRIBUTE_NAME_RECIEVED varchar(100) = N'I have received Jesus';

DECLARE @ATTRIBUTE_ID_OBEDIENCE int = 3935;
DECLARE @ATTRIBUTE_NAME_OBEDIENCE varchar(100) = N'Focusing on obedience - I''ve quit negotiating with God';

DECLARE @ATTRIBUTE_ID_REPLICATE int = 3936;
DECLARE @ATTRIBUTE_NAME_REPLICATE varchar(100) = N'I''m activtely replicating my faith in others.';

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

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_SEARCING)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_SEARCHING
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_SEARCING
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
				   (@ATTRIBUTE_ID_SEARCING
				   ,@ATTRIBUTE_NAME_SEARCHING
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END


IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_RECIEVED)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_RECIEVED
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_RECIEVED
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
				   (@ATTRIBUTE_ID_RECIEVED
				   ,@ATTRIBUTE_NAME_RECIEVED
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_OBEDIENCE)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_OBEDIENCE
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_OBEDIENCE
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
				   (@ATTRIBUTE_ID_OBEDIENCE
				   ,@ATTRIBUTE_NAME_OBEDIENCE
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_REPLICATE)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_REPLICATE
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_REPLICATE
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
				   (@ATTRIBUTE_ID_REPLICATE
				   ,@ATTRIBUTE_NAME_REPLICATE
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END
