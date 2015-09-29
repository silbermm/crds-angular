USE [MinistryPlatform]
GO

IF NOT EXISTS (Select 1 FROM [dbo].[dp_Communications] WHERE [dbo].[dp_Communications].[Communication_ID] = '12530')
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Communications] ON
INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID]
	   ,[Author_User_ID]
           ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[Expire_Date]
           ,[Communication_Status_ID]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[_Sent_From_Task]
           ,[Selection_ID]
           ,[Template]
           ,[Active]
           ,[To_Contact]
           ,[Time_Zone]
           ,[Locale])
     VALUES
           (12530
	   ,5
	   ,'You''ve Just Received Encouragement about Your GO Trip!'
	   ,'***Will Be Replaced By Donor''s Message***'
	   ,1
	   ,'2015-09-23 16:00:00.000'
	   ,NULL
	   ,1
	   ,7
	   ,7
	   ,NULL
	   ,NULL
	   ,1
	   ,1
	   ,NULL
	   ,NULL
	   ,NULL)
SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END
GO
