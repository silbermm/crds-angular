USE [MinistryPlatform]
GO	

ALTER TABLE [dbo].[Recurring_Gifts] add Consecutive_Failure_Count INT NOT NULL  DEFAULT ((0))
GO



UPDATE [dbo].[dp_Page_Views]
   SET [Field_List] = 'Recurring_Gifts.[Recurring_Gift_ID],Subscription_ID,Donor_ID_Table.Donor_ID,Program_ID,Congregation_ID,Frequency_ID,Amount,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type_ID],Donor_Account_ID_Table.Donor_Account_ID,Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type],Consecutive_Failure_Count'
      
 WHERE Page_View_ID = 2182
GO