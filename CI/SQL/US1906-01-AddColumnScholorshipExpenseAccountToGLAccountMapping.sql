USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donations]    Script Date: 8/12/2015 2:07:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Remove columns if they are there
IF COL_LENGTH('dbo.GL_Account_Mapping','Scholarship_Expense_Account') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Scholarship_Expense_Account];
END

-- Add new columns
ALTER TABLE [dbo].[GL_Account_Mapping]
ADD [Scholarship_Expense_Account] nvarchar(15);

GO

USE [MinistryPlatform]
GO

-- Load with dummy values
-- For Nicaragua but Default for testing
UPDATE [dbo].[GL_Account_Mapping]
SET [Scholarship_Expense_Account] = '75001-260-01';

-- For South Africa
UPDATE [dbo].[GL_Account_Mapping]
SET [Scholarship_Expense_Account] = '75001-220-01'
WHERE [Program_ID] = 99;

-- For India
UPDATE [dbo].[GL_Account_Mapping]
SET [Scholarship_Expense_Account] = '75001-240-01'
WHERE [Program_ID] = 117;

-- For NOLA
UPDATE [dbo].[GL_Account_Mapping]
SET [Scholarship_Expense_Account] = '75001-280-01'
WHERE [Program_ID] = 14;

GO
