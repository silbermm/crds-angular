USE [MinistryPlatform]
GO

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
           ,[To_Contact])
     VALUES
           (12666
		   ,5
           ,'Problem saving your Serve Request'
           ,'There was a problem trying to save your Signup To Serve request. Please log in at https://crossroads.net/serve-signup and 
		   resign up. If problems persist, please contact your administrator. <br /> <br /> What you tried to signup for: <br /> [HTML_Table] Thanks, <br /> Your friendly Crossroads Team'
           ,1
           ,GETDATE()
           ,NULL
           ,NULL
           ,7
           ,7
           ,NULL
           ,NULL
           ,1
           ,1
           ,NULL)
GO

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF