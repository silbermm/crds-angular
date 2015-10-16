USE [MinistryPlatform]
GO

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
  (  13020
    ,5
    ,'Your Recurring Gift has been removed due to several failed attempts'
    ,'<div>Your Recurring Gift has been removed for <program name> due to several failed attempts. </div><div><br /></div><div>If you have new account information or you can provide us with additional information regarding the cancellation of your Recurring Gift, we’d love to hear from you. Please email finance@crossroads.net and someone will get back to you shortly. </div><div><br /></div><div><div><b>You Were Giving To:</b> <span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);"> </span><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Program_Name]</span></div><div><b>Amount:</b>  <span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">$[Donation_Amount]</span></div><div><b>Frequency: </b> [Frequency]</div><div><b>Date Attempted:  </b><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Donation_Date]</span></div><div><b>Payment Method:  </b><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Payment_Method]</span></div></div><div><br /></div><div>Thank You - Crossroads</div>'
    ,1
    ,CAST('20151001 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO
