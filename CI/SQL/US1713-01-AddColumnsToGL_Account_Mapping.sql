USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donations]    Script Date: 8/12/2015 2:07:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Remove columns if they are there
ALTER TABLE [dbo].[GL_Account_Mapping]
DROP COLUMN Checkbook_ID, Cash_Account, Receivable_Account, Distribution_Account,
	Document_Type, Customer_ID;

-- Add new columns
ALTER TABLE [dbo].[GL_Account_Mapping]
ADD Checkbook_ID nvarchar(50), Cash_Account nvarchar(15), Receivable_Account nvarchar(15),
  Distribution_Account nvarchar(50), Document_Type nvarchar(50), Customer_ID nvarchar(50);

-- Load with dummy values
UPDATE [dbo].[GL_Account_Mapping]
SET [Checkbook_ID] = 'PNC01', [Cash_Account] = '01111-000-00',
  [Receivable_Account] = '01201-000-00', [Distribution_Account] = '40010-010-02',
  [Document_Type] = 'SALES', [Customer_id] = 'CONTRIBUTI001';

GO
