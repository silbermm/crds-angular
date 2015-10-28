USE [MinistryPlatform]
GO

DECLARE @Communication_ID INT = 13304;
DECLARE @Author_User_ID INT = 5;
DECLARE @Subject VARCHAR(256) = 'Thank you for signing up for Recurring Gift';
DECLARE @Body VARCHAR(max) = '<div>Thank you for signing up for a Recurring Gift!</div><div><br /></div><div>Your generosity enables real life change. Because of you, people are encountering God for the first time; people in our city and across the globe are receiving relief and care; churches around the world are receiving top-notch creative materials for free; relationships are being restored; and individual lives are being transformed.</div><div><br /></div><div>The information below has been setup in our system and your Recurring Gift will begin immediately:</div><div><br /></div><div><b>You''re Giving to</b>: [Program_Name]</div><div><b>Amount</b>: $[Donation_Amount]<span class="Apple-tab-span" style="white-space:pre">	</span></div><div><b>Frequency</b>: [Frequency]</div><div><b>Date</b>: [Donation_Date]</div><div><b>Payment Method</b>: [Payment_Method]</div><div><br /></div><div>If at any point you have questions, please contact our Finance team at <a href="mailto:finance@crossroads.net">finance@crossroads.net</a>.</div><div><br /></div><div>Note: This is not a receipt. This giving will be recorded as part of your regular giving and will be included on your quarterly statement for tax purposes. If you do not currently receive a quarterly statement, you will begin to receive one now.</div><div><br /></div><div>Thanks again for being a generous part of the team! - Crossroads</div>';
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
