USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Pages]
SET [Default_Field_List] = CONCAT([Default_Field_List], ',Donation_Status_ID_Table.Donation_Status
,Donations.Donation_Status_Date
,Donations.Transaction_Code')
WHERE [Sub_Page_ID] = 274;
