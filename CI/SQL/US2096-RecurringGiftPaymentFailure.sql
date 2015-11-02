USE [MinistryPlatform]
GO	

IF COL_LENGTH('dbo.Recurring_Gifts', 'Consecutive_Failure_Count') IS NULL
BEGIN
  ALTER TABLE [dbo].[Recurring_Gifts] add Consecutive_Failure_Count INT NOT NULL  DEFAULT ((0));
END;

IF (NOT EXISTS (SELECT 1
                FROM [dbo].[dp_Page_Views]
                WHERE [Page_View_ID] = 2182
                AND [Field_List] LIKE '%Consecutive_Failure_Count%'))
BEGIN
  UPDATE [dbo].[dp_Page_Views]
  SET [Field_List] = CONCAT([Field_List], ',Consecutive_Failure_Count')
  WHERE Page_View_ID = 2182;
END;

DECLARE @Communication_ID INT = 13002;
DECLARE @Author_User_ID INT = 5;
DECLARE @Subject VARCHAR(256) = 'Your Recurring Gift transaction failed';
DECLARE @Body VARCHAR(max) = 'It appears there was an issue processing your Recurring Gift. Please login to your account on <a href="http://crossroads.net">crossroads.net</a> and <a href="https://www.crossroads.net/profile">update your Recurring Gift</a> account information. If your Recurring Gift fails more than twice we will automatically remove your Recurring Gift from our system and stop attempting to charge the account. If at any point you have questions, please contact our Finance team at <a href="mailto:finance@crossroads.net">finance@crossroads.net</a>.<div><br /></div><div><div><b>You''re Giving To:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);"></span><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Program_Name]</span></div><div><b>Amount:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">$[Donation_Amount]</span></div><div><b>Frequency:</b>&nbsp;[Frequency]</div><div><b>Date Attempted:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Donation_Date]</span></div><div><b>Payment Method:</b>&nbsp;<span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Payment_Method]</span></div><div><br/></div><div>Thanks again for being a generous part of the team! - Crossroads</div></div>';
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

