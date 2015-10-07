USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donations]    Script Date: 8/12/2015 2:07:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Remove columns if they are there
IF COL_LENGTH('dbo.GL_Account_Mapping','Checkbook_ID') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Checkbook_ID];
END
IF COL_LENGTH('dbo.GL_Account_Mapping','Cash_Account') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Cash_Account];
END
IF COL_LENGTH('dbo.GL_Account_Mapping','Receivable_Account') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Receivable_Account];
END
IF COL_LENGTH('dbo.GL_Account_Mapping','Distribution_Account') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Distribution_Account];
END
IF COL_LENGTH('dbo.GL_Account_Mapping','Document_Type') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Document_Type];
END
IF COL_LENGTH('dbo.GL_Account_Mapping','Customer_ID') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[GL_Account_Mapping] DROP COLUMN [Customer_ID];
END

-- Add new columns
ALTER TABLE [dbo].[GL_Account_Mapping]
ADD [Checkbook_ID] nvarchar(50), [Cash_Account] nvarchar(15), [Receivable_Account] nvarchar(15),
  [Distribution_Account] nvarchar(50), [Document_Type] nvarchar(50), [Customer_ID] nvarchar(50);

-- Load with dummy values
UPDATE [dbo].[GL_Account_Mapping]
SET [Checkbook_ID] = 'PNC01', [Cash_Account] = '01111-000-00',
  [Receivable_Account] = '01201-000-00', [Distribution_Account] = '40010-010-02',
  [Document_Type] = 'SALES', [Customer_ID] = 'CONTRIBUTI001';

GO
