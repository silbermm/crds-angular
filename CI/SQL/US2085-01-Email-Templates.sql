USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 12986;

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
     12986
    ,5
    ,'Congrats! You are now a trip participant'
    ,'The trip admin has just added you to the trip!'
    ,1
    ,GetDate()
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
