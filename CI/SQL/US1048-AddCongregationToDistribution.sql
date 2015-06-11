USE [MinistryPlatform]
GO

ALTER TABLE [dbo].[Donation_Distributions] ADD
	[Congregation_ID] INT NULL
GO

ALTER TABLE [dbo].[Donation_Distributions]  
	WITH CHECK ADD  
	CONSTRAINT [FK_Donation_Distributions_Congregations] 
	FOREIGN KEY([Congregation_ID])
	REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

