USE [MinistryPlatform]
GO
SET IDENTITY_INSERT [dbo].[dp_Tools] ON

INSERT INTO [dbo].[dp_Tools]
           ([Tool_ID]
		   ,[Tool_Name]
           ,[Description]
           ,[Launch_Page])
     VALUES
           (42
		   ,'Trip Participants'
           ,'Trip Participants Tool'
           ,'http://demo.crossroads.net/mptools/tripParticipants')

SET IDENTITY_INSERT [dbo].[dp_Tools] OFF
GO