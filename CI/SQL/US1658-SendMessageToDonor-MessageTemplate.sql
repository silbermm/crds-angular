USE [MinistryPlatform]
GO

IF EXISTS(select 1 from [dbo].[dp_Communications] where [Communication_ID] = 12599)
	BEGIN
		UPDATE [dbo].[dp_Communications] set [Subject] = '[TripName]', [Body] = '[DonorMessage]' where [Communication_ID] = 12599
	END
ELSE
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
			   (
			   12599
			   ,1
			   ,'[TripName]'
			   ,'[DonorMessage]'
			   ,1
			   ,GETDATE()	
			   ,NULL	
			   ,1	
			   ,2130779	
			   ,2130779	
			   ,NULL	
			   ,NULL	
			   ,1	
			   ,1	
			   ,NULL	
			   ,NULL	
			   ,NULL
			   )
		SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
	END


