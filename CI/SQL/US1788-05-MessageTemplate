USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Communications] ON
GO

INSERT INTO [dbo].[dp_Communications]
           ([Communication_ID], [Author_User_ID]
           ,[Subject]
           ,[Body]
           ,[Domain_ID]
           ,[Start_Date]
           ,[Communication_Status_ID]
           ,[From_Contact]
           ,[Reply_to_Contact]
           ,[Template]
           ,[Active])
     VALUES
           (12302, 5
           ,'[TripTitle] GO Trip Invitation'
           ,'<div>[ParticipantName],</div><div><br /></div><div>Please use the link below to complete the [TripTitle] application.  The email address you register with on crossroads.net must match the email address that received this message.</div><div><br /></div><div>http://[BaseUrl]/trips/[PledgeCampaignID]/signup?invite=[InviteGUID]</div>'
           ,1
           ,'2015-08-25 03:34:00.000'
           ,1
           ,7
           ,7
           ,1
           ,1)
GO


SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
