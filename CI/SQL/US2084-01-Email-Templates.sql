USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12953;

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
     12953
    ,5
    ,'Trip Application Received'
    ,'Your Trip Application has been received.'
    ,1
    ,CAST('20150801 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12954;

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
     12954
    ,5
    ,'Trip Application Error'
    ,'Trip Application Error.'
    ,1
    ,CAST('20150801 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12955;

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
     12955
    ,5
    ,'Trip Application Error Applicant'
    ,'Trip Application Error.'
    ,1
    ,CAST('20150801 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
