USE [MinistryPlatform]
GO

DELETE FROM [dp_Communications] WHERE [Communication_ID] = 13714;

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
     13714
    ,1
    ,'Event RSVP'
    ,'<div>Thank you for RSVPing to [Event_Name]! Below is the list of people that you have signed up. <br /> <br /> [HTML_Table] <br /><br /> [Childcare] </div>'
    ,1
    ,GetDate()
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
