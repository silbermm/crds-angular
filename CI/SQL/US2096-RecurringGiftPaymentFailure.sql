USE [MinistryPlatform]
GO	

ALTER TABLE [dbo].[Recurring_Gifts] add Consecutive_Failure_Count INT NOT NULL  DEFAULT ((0))
GO



UPDATE [dbo].[dp_Page_Views]
   SET [Field_List] = 'Recurring_Gifts.[Recurring_Gift_ID],Subscription_ID,Donor_ID_Table.Donor_ID,Program_ID,Congregation_ID,Frequency_ID,Amount,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID],Donor_Account_ID_Table.Donor_Account_ID,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type],Consecutive_Failure_Count'
      
 WHERE Page_View_ID = 2182
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
  (
     13002
    ,5
    ,'Your Recurring Gift transaction failed'
    ,'It appears there was an issue processing your recurring Recurring Gift. Please login to your account on <a href="http://crossroads.net">crossroads.net</a> and <a href="https://www.crossroads.net/my/give/eGift.php">update your Recurring Gift</a> account information. If your Recurring Gift fails more than twice we will automatically remove your Recurring Gift from our system and stop attempting to charge the account. If at any point you have questions, please contact our Finance team at finance@crossroads.net.<div><br /></div><div><div><b>You’re Giving To:</b> <span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);"> </span><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Program_Name]</span></div><div><b>Amount:</b>  <span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">$[Donation_Amount]</span></div><div><b>Frequency: </b> [Frequency]</div><div><b>Date Attempted:  </b><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Donation_Date]</span></div><div><b>Payment Method:  </b><span style="font-family: Verdana; font-size: 12px; line-height: 18px; background-color: rgb(255, 255, 255);">[Payment_Method]</span></div><div><br /></div><div>Thanks again for being a generous part of the team! - Crossroads</div></div>'
    ,1
    ,CAST('20151001 00:00:00.000' as DATETIME)
    ,7
    ,7
    ,1
    ,1
  );

SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
GO

