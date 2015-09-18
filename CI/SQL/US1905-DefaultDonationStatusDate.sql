USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donations] ADD CONSTRAINT
	[DF_Donations_Default_Donation_Status_Date] DEFAULT GETDATE() FOR [Donation_Status_Date];
