USE [MinistryPlatform]
GO

IF NOT EXISTS (
  SELECT 1
  FROM [dbo].[dp_Sub_Pages]
  WHERE [Sub_Page_ID] = 274
  AND [Default_Field_List] LIKE '%.Donation_Status%.Donation_Status_Date%.Transaction_Code'
  )
BEGIN
  UPDATE [dbo].[dp_Sub_Pages]
  SET [Default_Field_List] = CONCAT([Default_Field_List], ',Donation_Status_ID_Table.Donation_Status
  ,Donations.Donation_Status_Date
  ,Donations.Transaction_Code')
  WHERE [Sub_Page_ID] = 274;
END
