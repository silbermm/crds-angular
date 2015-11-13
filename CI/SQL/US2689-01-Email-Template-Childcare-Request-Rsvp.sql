USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 13362;

SET IDENTITY_INSERT [dbo].[dp_Communications] ON
GO

INSERT INTO [dp_Communications]
  (
     [Communication_ID]
    ,[Author_User_ID]
    ,[Subject]
    ,[Body]
    ,[Domain_ID]
    ,[Start_Date]
    ,[From_Contact]
    ,[Reply_to_Contact]
    ,[Template]
    ,[Active]
  )
VALUES
  (
     13362
    ,1
    ,'Childcare RSVP'
    ,'<div>When you signed up for [GroupName] at Crossroads, you indicated you would need childcare!</div><div><br /></div><div>Use <a href="http://[BaseUrl]/childcare/[EventId]">this link</a> to tell us which children will be coming with you on [EventStartDate] so we can properly plan to have the right staff available for each age group.</div>'
    ,1
    ,GetDate()
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
