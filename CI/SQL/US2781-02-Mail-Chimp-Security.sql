USE MinistryPlatform
GO

-- Add read / write permissions for API user to Publication page
IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Pages] WHERE [Role_Page_ID] = 13047)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] ON 

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_Page_ID]
           ,[Role_ID]
           ,[Page_ID]
           ,[Access_Level]
		   ,[Scope_All]
		   ,[Approver]
		   ,[File_Attacher]
		   ,[Data_Importer]
		   ,[Data_Exporter]
		   ,[Secure_Records]
		   ,[Allow_Comments]
		   ,[Quick_Add])
     VALUES
           (13047
           ,62
           ,376
           ,2
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] OFF


END
GO

-- Add read / write permissions for API user to Publication page
IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Pages] WHERE [Role_Page_ID] = 13048)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] ON 

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_Page_ID]
           ,[Role_ID]
           ,[Page_ID]
           ,[Access_Level]
		   ,[Scope_All]
		   ,[Approver]
		   ,[File_Attacher]
		   ,[Data_Importer]
		   ,[Data_Exporter]
		   ,[Secure_Records]
		   ,[Allow_Comments]
		   ,[Quick_Add])
     VALUES
           (13048
           ,62
           ,535
           ,2
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] OFF


END
GO

-- Add read permissions for API user to All Subscriptions page
IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Pages] WHERE [Role_Page_ID] = 13049)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] ON 

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_Page_ID]
           ,[Role_ID]
           ,[Page_ID]
           ,[Access_Level]
		   ,[Scope_All]
		   ,[Approver]
		   ,[File_Attacher]
		   ,[Data_Importer]
		   ,[Data_Exporter]
		   ,[Secure_Records]
		   ,[Allow_Comments]
		   ,[Quick_Add])
     VALUES
           (13049
           ,62
           ,417
           ,1
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Role_Pages] OFF


END
GO

-- Add read permissions for API user to Publication Page Views sub page
IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Sub_Pages] WHERE [Role_Sub_Page_ID] = 16571)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] ON 

INSERT INTO [dbo].[dp_Role_Sub_Pages]
           (
		   [Role_Sub_Page_ID]
           ,[Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level]
		   )
     VALUES
           (16571
           ,62
           ,543
           ,1
		   )

SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] OFF

END
GO

-- Grant Setup Admin to the API User to allow access to the Page View page
UPDATE dbo.dp_Users 
	SET Setup_Admin = 1 
	WHERE User_Name = 'register_api'