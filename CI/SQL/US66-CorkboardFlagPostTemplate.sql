USE [MinistryPlatform]
GO

DELETE FROM [dbo].[dp_Communications]
      WHERE [Communication_ID] = 11820;
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
          (11820
          ,1824433
          ,'Corkboard Flagged Post: [Title]'
          ,'This post has been flagged 6 times, please review it.<br /><br />Click <a href="[Link]"> here to view the post</a>'
          ,1
          ,GETDATE()
          ,NULL
          ,1
          ,7
          ,7
          ,NULL
          ,NULL
          ,1
          ,1
          ,NULL)
SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
