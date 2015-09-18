USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
SET [Default_Field_List] =
'Donation_ID_Table.Donation_Date
,ISNULL(Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Last_Name,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.Display_Name) AS [Last_Name]
,Donation_ID_Table_Donor_ID_Table_Contact_ID_Table.First_Name
,Donation_Distributions.Amount
,Donation_ID_Table_Payment_Type_ID_Table.Payment_Type
,Donation_ID_Table.Item_Number
,Program_ID_Table.Statement_Title
,Pledge_ID_Table_Pledge_Campaign_ID_Table.Campaign_Name
,Donation_Distributions.Donation_ID
,Congregation_ID_Table.Congregation_Name AS [Congregation]
,Donation_ID_Table.Donor_ID
,Donation_ID_Table_Batch_ID_Table.Batch_ID
,Pledge_ID_Table.Pledge_ID
,Donation_ID_Table.Donation_Status_Date
,Donation_ID_Table.Donation_Status_ID
,Donation_ID_Table.Transaction_Code
,Donation_ID_Table.Payment_Type_ID'
WHERE [Page_ID] = 296;
