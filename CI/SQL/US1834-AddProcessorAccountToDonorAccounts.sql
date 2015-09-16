USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donor_Accounts] ADD
	[Processor_Account_ID] nvarchar(50) NULL,
	[Processor_ID] nvarchar(255) NULL
GO
