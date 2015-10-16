USE [MinistryPlatform]
GO

ALTER TABLE dbo.Attributes ALTER COLUMN [Attribute_Name] nvarchar(100) not null
GO

-- ATTRIBUTE TYPE DETAILS
DECLARE @ATTRIBUTE_TYPE_ID int = 63;
DECLARE @ATTRIBUTE_TYPE_VALUE varchar(50) = N'FrequentFlyer';
DECLARE @ATTRIBUTE_TYPE_DESCRIPTION varchar(255) = N'Record your frequent flyer airlines';

DECLARE @ATTRIBUTE_ID_DELTA int = 3958;
DECLARE @ATTRIBUTE_NAME_DELTA varchar(100) = N'Delta Airlines';

DECLARE @ATTRIBUTE_ID_SA int = 3959;
DECLARE @ATTRIBUTE_NAME_SA varchar(100) = N'South Africa Airlines';

DECLARE @ATTRIBUTE_ID_UNITED int = 3960;
DECLARE @ATTRIBUTE_NAME_UNITED varchar(100) = N'United Airlines';

DECLARE @ATTRIBUTE_ID_US int = 3980;
DECLARE @ATTRIBUTE_NAME_US varchar(100) = N'US Airlines';

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

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_DELTA)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_DELTA
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_DELTA
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
				   (@ATTRIBUTE_ID_DELTA
				   ,@ATTRIBUTE_NAME_DELTA
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END


IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_SA)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_SA
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_SA
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
				   (@ATTRIBUTE_ID_SA
				   ,@ATTRIBUTE_NAME_SA
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_UNITED)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_UNITED
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_UNITED
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
				   (@ATTRIBUTE_ID_UNITED
				   ,@ATTRIBUTE_NAME_UNITED
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END

IF EXISTS (Select 1 FROM [dbo].[Attributes] WHERE [Attribute_ID] = @ATTRIBUTE_ID_US)
	BEGIN
		UPDATE [dbo].[Attributes]
		   SET [Attribute_Name] = @ATTRIBUTE_NAME_US
			   ,[Domain_ID] = 1
			   ,[Attribute_Type_ID] = @ATTRIBUTE_TYPE_ID
		 WHERE [dbo].Attributes.Attribute_ID = @ATTRIBUTE_ID_US
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
				   (@ATTRIBUTE_ID_US
				   ,@ATTRIBUTE_NAME_US
				   ,1
				   ,@ATTRIBUTE_TYPE_ID)
			SET IDENTITY_INSERT [dbo].[Attributes] OFF
	END