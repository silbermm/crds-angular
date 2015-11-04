USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] =
    'Donations.Donation_Date
	,Donor_ID_Table_Contact_ID_Table.Display_Name
	,Donor_ID_Table_Contact_ID_Table.Nickname
	,Donor_ID_Table_Contact_ID_Table.First_Name
	,Donations.Donation_Amount
	,Payment_Type_ID_Table.Payment_Type
	,Item_Number
	,Transaction_Code
	,Subscription_Code
	,Batch_ID_Table.Batch_ID
	,Batch_ID_Table.Setup_Date
	,Donations.Registered_Donor
	,Processor_Fee_Amount
	,Donor_ID_Table.Donor_ID
	,Donation_Status_ID_Table.Donation_Status
	,Donation_Status_Notes
	,Donations.Check_Scanner_Batch
	,Is_Recurring_Gift'
 WHERE page_id = 297
 GO