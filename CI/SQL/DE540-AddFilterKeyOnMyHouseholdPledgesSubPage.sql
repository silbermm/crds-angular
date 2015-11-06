USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Pages]
SET [Filter_Key] = 'Donor_ID'
WHERE [Sub_Page_ID] = 540;