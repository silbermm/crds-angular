USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donor_Accounts] ADD
	[Encrypted_Account] nvarchar(255) NULL;
