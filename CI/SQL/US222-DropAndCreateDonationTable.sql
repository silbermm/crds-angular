USE [MinistryPlatform]

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Donations', @level2type=N'COLUMN',@level2name=N'Payment_Type_ID'

GO

PRINT N'Dropping [dbo].[DF__Donations__Recei__5DCBBABB]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [DF__Donations__Recei__5DCBBABB];
GO

PRINT N'Dropping [dbo].[FK_Donations_dp_Domains]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_dp_Domains]
GO

PRINT N'Dropping [dbo].[FK_Donations_Donors]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Donors]
GO

PRINT N'Dropping [dbo].[FK_Donations_Donor_Accounts]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Donor_Accounts]
GO

PRINT N'Dropping [dbo].[FK_Donations_Donation_Types]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Donation_Types]
GO

PRINT N'Dropping [dbo].[FK_Donations_Donation_Statuses]...';
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donations_Donation_Statuses]') AND parent_object_id = OBJECT_ID(N'[dbo].[Donations]'))
	ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Donation_Statuses]
GO

PRINT N'Dropping [dbo].[FK_Donations_Donation_Non_Cash_Assets]...';
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Donations_Donation_Non_Cash_Assets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Donations]'))
	ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Donation_Non_Cash_Assets]
GO

PRINT N'Dropping [dbo].[FK_Donations_Batches]...';
GO
ALTER TABLE [dbo].[Donations] DROP CONSTRAINT [FK_Donations_Batches]
GO

GO
PRINT N'Dropping [dbo].[FK_Donation_Distributions_Donations]...';
GO
ALTER TABLE [dbo].[Donation_Distributions] DROP CONSTRAINT [FK_Donation_Distributions_Donations];

GO
PRINT N'Starting rebuilding table [dbo].[Donations]...';
GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Donations](
	[Donation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Donor_ID] [int] NOT NULL,
	[Donation_Amount] [money] NOT NULL,
	[Donation_Date] [datetime] NOT NULL,
	[Payment_Type_ID] [int] NOT NULL,
	[Non_Cash_Asset_Type_ID] [int] NULL,
	[Item_Number] [nvarchar](15) NULL,
	[Batch_ID] [int] NULL,
	[Notes] [nvarchar](500) NULL,
	[Donor_Account_ID] [int] NULL,
	[Anonymous] [bit] NULL,
	[Donation_Status_Information] [dbo].[dp_Separator] NULL,
	[Donation_Status_ID] [int] NULL,
	[Donation_Status_Date] [datetime] NULL,
	[Donation_Status_Notes] [nvarchar](500) NULL,
	[Online_Donation_Information] [dbo].[dp_Separator] NULL,
	[Transaction_Code] [nvarchar](50) NULL,
	[Subscription_Code] [nvarchar](50) NULL,
	[Gateway_Response] [nvarchar](500) NULL,
	[Processed] [bit] NULL,
	[Domain_ID] [int] NOT NULL,
	[Currency] [nvarchar](25) NULL,
	[Receipted] [bit] NOT NULL CONSTRAINT [DF__Donations__Recei__5DCBBABB]  DEFAULT ((0)),
	[Invoice_Number] [nvarchar](25) NULL,
	[Receipt_Number] [int] NULL,
	[__ExternalContributionID] [int] NULL,
	[__ExternalPaymentID] [int] NULL,
	[__ExternalGiverID] [int] NULL,
	[__ExternalDonorID] [int] NULL,
	[__ExteralMasterID1] [int] NULL,
	[__ExternalMasterID2] [int] NULL,
	[Registered_Donor] [bit] NULL,
	[Processor_ID] [nvarchar](50) NULL,
	[Processor_Fee_Amount] [money] NULL,
 CONSTRAINT [tmp_ms_xx_constraint_PK_Donations] PRIMARY KEY CLUSTERED 
(
	[Donation_ID] ASC
)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Donations])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Donations] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Donations] 
			(
				[Donation_ID],
				[Donor_ID],
				[Donation_Amount],
				[Donation_Date],
				[Payment_Type_ID],
				[Non_Cash_Asset_Type_ID],
				[Item_Number],
				[Batch_ID],
				[Notes],
				[Donor_Account_ID],
				[Anonymous],
				[Donation_Status_Information], 
				[Donation_Status_ID],
				[Donation_Status_Date],
				[Donation_Status_Notes],
				[Online_Donation_Information],
				[Transaction_Code],
				[Subscription_Code],
				[Gateway_Response],
				[Processed],
				[Domain_ID],
				[Currency],
				[Receipted],
				[Invoice_Number],
				[Receipt_Number],
				[__ExternalContributionID],
				[__ExternalPaymentID],
				[__ExternalGiverID],
				[__ExternalDonorID],
				[__ExteralMasterID1],
				[__ExternalMasterID2],
				[Registered_Donor],
				[Processor_ID],
				[Processor_Fee_Amount]
			)
        SELECT   
				[Donation_ID],
				[Donor_ID],
				[Donation_Amount],
				[Donation_Date],
				[Payment_Type_ID],			
				NULL, --[Non_Cash_Asset_Type_ID]
				[Item_Number],
				[Batch_ID],
				[Notes],
				[Donor_Account_ID],
				[Anonymous],
				NULL,  -- [Donation_Status_Information]
				[Donation_Status_ID],
				[Donation_Status_Date],
				[Donation_Status_Notes],
				[Online_Donation_Information],
				[Transaction_Code],
				[Subscription_Code],
				[Gateway_Response],
				[Processed],
				[Domain_ID],
				[Currency],
				[Receipted],
				[Invoice_Number],
				[Receipt_Number],
				[__ExternalContributionID],
				[__ExternalPaymentID],
				[__ExternalGiverID],
				[__ExternalDonorID],
				[__ExteralMasterID1],
				[__ExternalMasterID2],
				[Registered_Donor],
				[Processor_ID],
				[Processor_Fee_Amount]
        FROM     [dbo].[Donations]
        ORDER BY [Donation_ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Donations] OFF;
    END

DROP TABLE [dbo].[Donations];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Donations]', N'Donations';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Donations]', N'PK_Donations', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO

PRINT N'Creating [dbo].[Donations].[IX_BatchID]...';
GO
CREATE NONCLUSTERED INDEX [IX_BatchID]
    ON [dbo].[Donations]([Batch_ID] ASC);
GO

PRINT N'Creating [dbo].[Donations].[IX_Donations_ExternalContributionID]...';
GO

CREATE NONCLUSTERED INDEX [IX_Donations_ExternalContributionID]
    ON [dbo].[Donations]([__ExternalContributionID] ASC);
GO




/****** Object:  Table [dbo].[Donations]    Script Date: 8/4/2015 11:10:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


PRINT N'Creating [dbo].[FK_Donations_Batches]...';
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Batches] FOREIGN KEY([Batch_ID])
REFERENCES [dbo].[Batches] ([Batch_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Batches]
GO

PRINT N'Creating [dbo].[FK_Donations_Donation_Non_Cash_Assets]...';
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donation_Non_Cash_Assets] FOREIGN KEY([Non_Cash_Asset_Type_ID])
REFERENCES [dbo].[Donation_Non_Cash_Assets] ([Non_Cash_Asset_Type_ID])
GO

PRINT N'Creating [dbo].[FK_Donation_Distributions_Donations]...';
GO
ALTER TABLE [dbo].[Donation_Distributions] WITH NOCHECK ADD CONSTRAINT [FK_Donation_Distributions_Donations] FOREIGN KEY ([Donation_ID]) 
REFERENCES [dbo].[Donations] ([Donation_ID]);
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donation_Non_Cash_Assets]
GO

PRINT N'Creating [dbo].[FK_Donations_Donation_Statuses]...';
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donation_Statuses] FOREIGN KEY([Donation_Status_ID])
REFERENCES [dbo].[Donation_Statuses] ([Donation_Status_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donation_Statuses]
GO

PRINT N'Creating [dbo].[FK_Donations_Donation_Types]...';
GO

ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donation_Types] FOREIGN KEY([Payment_Type_ID])
REFERENCES [dbo].[Payment_Types] ([Payment_Type_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donation_Types]
GO

PRINT N'Creating [dbo].[FK_Donations_Donor_Accounts]...';
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donor_Accounts] FOREIGN KEY([Donor_Account_ID])
REFERENCES [dbo].[Donor_Accounts] ([Donor_Account_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donor_Accounts]
GO


PRINT N'Creating [dbo].[FK_Donations_Donors]...';
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Donors] FOREIGN KEY([Donor_ID])
REFERENCES [dbo].[Donors] ([Donor_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Donors]
GO

PRINT N'Creating [dbo].[FK_Donations_dp_Domains]...';
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_dp_Domains]
GO

PRINT N'Creating [dbo].[Donations].[Payment_Type_ID].[MS_Description]...';
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Non Cash Items will NOT appear on a donor statement.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Donations', @level2type=N'COLUMN',@level2name=N'Payment_Type_ID'
GO
