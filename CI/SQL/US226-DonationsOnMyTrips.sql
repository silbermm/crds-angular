USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Display_On_MyTrips' AND Object_ID = Object_ID(N'Donation_Statuses'))
BEGIN
	ALTER TABLE [dbo].[Donation_Statuses]
	ADD Display_On_MyTrips bit
END