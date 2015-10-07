USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] ON

INSERT INTO [dbo].[dp_Tool_Pages]
           ([Tool_Page_ID]
		   ,[Tool_ID]
           ,[Page_ID])
     VALUES
           (464
		   ,43
           ,424)

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] OFF
GO

SET IDENTITY_INSERT [dbo].[dp_Role_Tools] ON

INSERT INTO [dbo].[dp_Role_Tools]
           (Role_Tool_ID, Role_ID, Tool_ID, Domain_ID)
     VALUES
           (118, 2, 43, 1)
SET IDENTITY_INSERT [dbo].[dp_Role_Tools] OFF
GO