USE [MinistryPlatform]
GO

-- Donation_Status_ID 1 == "Pending"
ALTER TABLE [dbo].[Donations] ADD CONSTRAINT
	[DF_Donations_Donation_Status_ID] DEFAULT 1 FOR [Donation_Status_ID];
