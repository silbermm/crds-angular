USE [MinistryPlatform]
GO

DECLARE @Communication_ID INT = 13010;
DECLARE @Author_User_ID INT = 5;
DECLARE @Subject VARCHAR(256) = 'Your Recurring Gift has been removed due to several failed attempts';
DECLARE @Body VARCHAR(max) = '<div>Your Recurring Gift has been removed for [Program_Name] due to several failed attempts. </div><div><br/></div><div>If you have new account information or you can provide us with additional information regarding the cancellation of your Recurring Gift, we''d love to hear from you. Please email <a href="mailto:finance@crossroads.net">finance@crossroads.net</a> and someone will get back to you shortly. </div><div><br /></div><div><div><b>You Were Giving To:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);"> </span><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Program_Name]</span></div><div><b>Amount:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">$[Donation_Amount]</span></div><div><b>Frequency:</b>&nbsp;[Frequency]</div><div><b>Date Attempted:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Donation_Date]</span></div><div><b>Payment Method:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Payment_Method]</span></div></div><div><br /></div><div>Thank You - Crossroads</div>';
DECLARE @Domain_ID INT = 1;
DECLARE @Start_Date DATETIME = CAST('20151001 00:00:00.000' as DATETIME);
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
  WHERE [Communication_ID] = @Communication_ID
END;

