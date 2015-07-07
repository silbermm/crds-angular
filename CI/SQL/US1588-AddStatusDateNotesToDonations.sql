USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donations]
	ADD [Donation_Status_ID] [int] NULL,
		[Donation_Status_Date] [datetime] NULL,
		[Donation_Status_Notes] [nvarchar](500) NULL
GO

ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donation_Status] FOREIGN KEY([Donation_Status_ID])
REFERENCES [dbo].[Donation_Statuses] ([Donation_Status_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donation_Status]
GO