USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Pages] WHERE [Page_ID] = 12179)
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
           (12179
           ,39
           ,376
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