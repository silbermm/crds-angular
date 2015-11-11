USE [MinistryPlatform]
GO

UPDATE [dbo].[Accounting_Companies]
SET [Statement_Footer] = 'No goods or services were provided by Crossroads in exchange for the contributions recorded on this statement.'
WHERE [Accounting_Company_ID] = 1;