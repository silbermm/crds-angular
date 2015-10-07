USE [MinistryPlatform]
GO

DELETE FROM [dbo].[dp_Communications] 
       WHERE [Subject] = 'Volunteer Cancelation' AND [Template] = 1;
GO

INSERT INTO [dbo].[dp_Communications]
           ([Author_User_ID]
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
           (5
           ,'Volunteer Cancelation'
           ,'[VolunteerName] ([VolunteerEmail]) has cancelled their serving sign-up for [TeamName] - [OpportunityName], [EventDateTime]'
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

