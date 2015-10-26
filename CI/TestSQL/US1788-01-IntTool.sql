USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Tools]
   SET [Launch_Page] = 'http://int.crossroads.net/mptools/tripPrivateInvite'
 WHERE Tool_ID = 369
GO

SET IDENTITY_INSERT [dbo].[dp_Tools] ON
GO

INSERT INTO [dbo].[dp_Tools]
           ([Tool_ID]
		   ,[Tool_Name]
           ,[Description]
           ,[Launch_Page])
     VALUES
           (373
		   ,'Trip Private Invites - DEV'
           ,'Development Trip Private Invite Tool'
           ,'http://localhost:3000/mptools/tripPrivateInvite')

SET IDENTITY_INSERT [dbo].[dp_Tools] OFF
GO

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] ON

INSERT INTO [dbo].[dp_Tool_Pages]
           ([Tool_Page_ID]
		   ,[Tool_ID]
           ,[Page_ID])
     VALUES
           (480
		   ,373
           ,514)

SET IDENTITY_INSERT [dbo].[dp_Tool_Pages] OFF
GO

INSERT INTO [dbo].[dp_Role_Tools]
           (Role_ID, Tool_ID, Domain_ID)
     VALUES
           (2, 373, 1)
