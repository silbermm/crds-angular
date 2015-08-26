USE [MinistryPlatform]
GO

-- Remove columns if they are there
IF COL_LENGTH('dbo.Deposit_Amount','Deposit_Amount') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[Deposits] DROP COLUMN [Deposit_Amount];
END
IF COL_LENGTH('dbo.Processor_Fee_Total','Processor_Fee_Total') IS NOT NULL
BEGIN
  ALTER TABLE [dbo].[Deposits] DROP COLUMN [Processor_Fee_Total];
END

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Deposits_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Deposits]'))
ALTER TABLE [dbo].[Deposits] DROP CONSTRAINT [FK_Deposits_dp_Domains]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Batches_Deposits]') AND parent_object_id = OBJECT_ID(N'[dbo].[Batches]'))
ALTER TABLE [dbo].[Batches] DROP CONSTRAINT [FK_Batches_Deposits]
GO


CREATE TABLE [dbo].[tmp_Deposits](
	[Deposit_ID] [int] IDENTITY(1,1) NOT NULL,
	[Deposit_Name] [nvarchar](75) NOT NULL,
	[Deposit_Total] [money] NOT NULL,
	[Deposit_Amount] [money] NOT NULL,
	[Processor_Fee_Total] [money] NOT NULL,
	[Deposit_Date] [datetime] NOT NULL,
	[Account_Number] [nvarchar](15) NOT NULL,
	[Batch_Count] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Exported] [bit] NOT NULL CONSTRAINT [DF_tmpb_Deposits_Exported]  DEFAULT ((0)),
	[Notes] [nvarchar](500) NULL,
	[__ExternalBatchID] [int] NULL,
	[Processor_Transfer_ID] [nvarchar](100) NULL,
 CONSTRAINT [PK_tmp_Deposits] PRIMARY KEY CLUSTERED
(
	[Deposit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Copy data ******/
IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[Deposits])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_Deposits] ON;
        INSERT INTO [dbo].[tmp_Deposits]
			([Deposit_ID],
			[Deposit_Name],
			[Deposit_Total],
			[Deposit_Amount],
			[Processor_Fee_Total],
			[Deposit_Date],
			[Account_Number],
			[Batch_Count],
			[Domain_ID],
			[Exported],
			[Notes],
			[__ExternalBatchID],
			[Processor_Transfer_ID])
        SELECT
			[Deposit_ID],
			[Deposit_Name],
			[Deposit_Total],
			0,
			0,
			[Deposit_Date],
			[Account_Number],
			[Batch_Count],
			[Domain_ID],
			[Exported],
			[Notes],
			[__ExternalBatchID],
			[Processor_Transfer_ID]
        FROM     [dbo].[Deposits]
        ORDER BY [Deposit_ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_Deposits] OFF;
    END


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Deposits]') AND type in (N'U'))
DROP TABLE [dbo].[Deposits]
GO

EXECUTE sp_rename N'[dbo].[tmp_Deposits]', N'Deposits';

EXECUTE sp_rename N'[dbo].[PK_tmp_Deposits]', N'PK_Deposits', N'OBJECT';

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

EXEC sp_addextendedproperty
@name = N'MS_Description', @value = 'Total of all donation amounts included in deposit',
@level0type=N'Schema', @level0name = [dbo],
@level1type=N'Table', @level1name= [Deposits],
@level2type = N'Column', @level2name = [Deposit_Total];
GO
EXEC sp_addextendedproperty
@name = N'MS_Description', @value = 'Deposit from Payment Processor which is the donation total minus processor fees',
@level0type=N'Schema', @level0name = [dbo],
@level1type=N'Table', @level1name= [Deposits],
@level2type = N'Column', @level2name = [Deposit_Amount];
GO
EXEC sp_addextendedproperty
@name = N'MS_Description', @value = 'Payment Processor Fees only applicable to credit cards, bank and check donations',
@level0type=N'Schema', @level0name = [dbo],
@level1type=N'Table', @level1name= [Deposits],
@level2type = N'Column', @level2name = [Processor_Fee_Total];
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Deposits_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Deposits]'))
ALTER TABLE [dbo].[Deposits]  WITH CHECK ADD  CONSTRAINT [FK_Deposits_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Deposits_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Deposits]'))
ALTER TABLE [dbo].[Deposits] CHECK CONSTRAINT [FK_Deposits_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Batches_Deposits]') AND parent_object_id = OBJECT_ID(N'[dbo].[Batches]'))
ALTER TABLE [dbo].[Batches]  WITH CHECK ADD  CONSTRAINT FK_Batches_Deposits FOREIGN KEY([Deposit_ID])
REFERENCES [dbo].[Deposits] ([Deposit_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Batches_Deposits]') AND parent_object_id = OBJECT_ID(N'[dbo].[Batches]'))
ALTER TABLE [dbo].[Batches] CHECK CONSTRAINT [FK_Batches_Deposits]
GO
