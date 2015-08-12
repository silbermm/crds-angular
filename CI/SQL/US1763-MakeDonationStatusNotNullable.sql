USE [MinistryPlatform]
GO

UPDATE [dbo].[Donations]
SET    [Donation_Status_ID] = 4
WHERE  [Donation_Status_ID] IS NULL;

ALTER TABLE [dbo].[Donations] ALTER COLUMN [Donation_Status_ID] [int] NOT NULL;
