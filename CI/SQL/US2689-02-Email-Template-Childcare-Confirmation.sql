USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 13548;

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
     13548
    ,1
    ,'Childcare Confirmation'
    ,'<div>Thanks for letting us know your childcare needs for [EventTitle] meeting on [EventStartDate].</div><div>We''ve marked you down for</div> [ChildNames] <div>If something changes and you won''t be attending or your kids won''t be with you, please contact us at</div><ul><li>childcareoakley@crossroads.net for Oakely Groups</li><li>childcaremason@crossroads.net for Mason Groups</li><li>childcareflorence@crossroads.net for Florence Groups</li><li>childcarewestside@crossroads.net for West Side Groups</li></ul>'
    ,1
    ,GetDate()
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
