USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donations] ADD [Is_Recurring_Gift] [bit] NULL
GO

ALTER TABLE [dbo].[Donations] ADD [Recurring_Gift_ID] int NULL
GO

ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Recurring_Gifts] FOREIGN KEY([Recurring_Gift_ID])
REFERENCES [dbo].[Recurring_Gifts] ([Recurring_Gift_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Recurring_Gifts]
GO
