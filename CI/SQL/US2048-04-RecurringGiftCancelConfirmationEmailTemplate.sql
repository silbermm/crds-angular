USE [MinistryPlatform]
GO

DECLARE @Communication_ID INT = 13306;
DECLARE @Author_User_ID INT = 5;
DECLARE @Subject VARCHAR(256) = 'Your Recurring Gift has been removed';
DECLARE @Body VARCHAR(max) = '<div>We have removed your Recurring Gift for [Program_Name]. Thanks again for the real life change that your generosity has helped to fuel around here.</div><div><br/></div><div>If you''re willing to tell us why you''re cancelling your Recurring Gift, we''d love to hear from you. Please contact Addison Lanier at 513-731-7400 x1583 or <a href="mailto:alanier@crossroads.net">alanier@crossroads.net</a> to let us know, or to ask any questions.</div><div><br/></div><div>Thank You - Crossroads</div>';
DECLARE @Domain_ID INT = 1;
DECLARE @Start_Date DATETIME = CAST('20151027 00:00:00.000' as DATETIME);
DECLARE @From_Contact INT = 7;

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Communications]
                WHERE [Communication_ID] = @Communication_ID))
BEGIN
  SET IDENTITY_INSERT [dbo].[dp_Communications] ON;

  INSERT INTO [dbo].[dp_Communications]
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
       @Communication_ID
      ,@Author_User_ID
      ,@Subject
      ,@Body
      ,@Domain_ID
      ,@Start_Date
      ,@From_Contact
      ,@From_Contact
      ,1
      ,1
    );

  SET IDENTITY_INSERT [dbo].[dp_Communications] OFF;
END
ELSE
BEGIN
  UPDATE [dbo].[dp_Communications]
  SET [Author_User_ID] = @Author_User_ID
    , [Subject] = @Subject
    , [Body] = @Body
    , [Domain_ID] = @Domain_ID
    , [Start_Date] = @Start_Date
    , [From_Contact] = @From_Contact
    , [Reply_to_Contact] = @From_Contact
    , Template = 1
    , Active = 1
  WHERE [Communication_ID] = @Communication_ID
END;
