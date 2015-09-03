USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Tools] ON

INSERT INTO [dbo].[dp_Tools]
           ([Tool_ID]
		   ,[Tool_Name]
           ,[Description]
           ,[Launch_Page])
     VALUES
           (369
		   ,'Trip Private Invites'
           ,'Trip Private Invite Tool'
           ,'http://demo.crossroads.net/mptools/tripPrivateInvite')

SET IDENTITY_INSERT [dbo].[dp_Tools] OFF
GO

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] ON
GO

INSERT INTO [dbo].[dp_Tool_Pages]
           ([Tool_Page_ID]
		       ,[Tool_ID]
           ,[Page_ID])
     VALUES
           (474
		       ,369
           ,514)

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] OFF
GO

INSERT INTO [dbo].[dp_Role_Tools]
           (Role_ID, Tool_ID, Domain_ID)
     VALUES
           (2, 369, 1)
