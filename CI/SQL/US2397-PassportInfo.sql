USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Information')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Information] dp_Separator null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Firstname')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Firstname] varchar(100) null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Middlename')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Middlename] varchar(100) null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Lastname')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Lastname] varchar(100) null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Country')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Country] varchar(50) null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Expiration')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Expiration] date null;
	END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			WHERE  TABLE_NAME = 'Contacts'
            AND COLUMN_NAME = 'Passport_Number')
	BEGIN
		ALTER TABLE [dbo].[Contacts] ADD [Passport_Number] varchar(50) null;
	END




