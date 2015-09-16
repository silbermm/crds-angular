USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Programs] ADD
	[Allow_Recurring_Giving] [bit] NOT NULL DEFAULT (0);