USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donors]    Script Date: 4/29/2015 5:14:45 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Donors](
	[Donor_ID] [int] IDENTITY(1,1) NOT NULL,
	[Contact_ID] [int] NOT NULL,
	[Statement_Frequency_ID] [int] NOT NULL,
	[Statement_Type_ID] [int] NOT NULL CONSTRAINT [DF_Donors_Statement_Type_ID]  DEFAULT ((2)),
	[Statement_Method_ID] [int] NOT NULL,
	[Setup_Date] [datetime] NOT NULL,
	[Envelope_No] [int] NULL,
	[Cancel_Envelopes] [bit] NULL,
	[Notes] [nvarchar](500) NULL,
	[First_Contact_Made] [bit] NULL,
	[Domain_ID] [int] NOT NULL,
	[__ExternalPersonID] [int] NULL,
	[_First_Donation_Date] [datetime] NULL,
	[_Last_Donation_Date] [datetime] NULL,
	[Stripe_Customer_ID] [nvarchar](255) NULL,
 CONSTRAINT [PK_Donors] PRIMARY KEY CLUSTERED 
(
	[Donor_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Donors]  WITH CHECK ADD  CONSTRAINT [FK_Donors_Contacts] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

ALTER TABLE [dbo].[Donors] CHECK CONSTRAINT [FK_Donors_Contacts]
GO

ALTER TABLE [dbo].[Donors]  WITH CHECK ADD  CONSTRAINT [FK_Donors_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Donors] CHECK CONSTRAINT [FK_Donors_dp_Domains]
GO

ALTER TABLE [dbo].[Donors]  WITH CHECK ADD  CONSTRAINT [FK_Donors_Statement_Frequencies] FOREIGN KEY([Statement_Frequency_ID])
REFERENCES [dbo].[Statement_Frequencies] ([Statement_Frequency_ID])
GO

ALTER TABLE [dbo].[Donors] CHECK CONSTRAINT [FK_Donors_Statement_Frequencies]
GO

ALTER TABLE [dbo].[Donors]  WITH CHECK ADD  CONSTRAINT [FK_Donors_Statement_Methods] FOREIGN KEY([Statement_Method_ID])
REFERENCES [dbo].[Statement_Methods] ([Statement_Method_ID])
GO

ALTER TABLE [dbo].[Donors] CHECK CONSTRAINT [FK_Donors_Statement_Methods]
GO

ALTER TABLE [dbo].[Donors]  WITH CHECK ADD  CONSTRAINT [FK_Donors_Statement_Types] FOREIGN KEY([Statement_Type_ID])
REFERENCES [dbo].[Statement_Types] ([Statement_Type_ID])
GO

ALTER TABLE [dbo].[Donors] CHECK CONSTRAINT [FK_Donors_Statement_Types]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AVOID MANUALLY ADDING A NEW NUMBER.  Use The Assgin Next Envelop No Tool instead.  Feel free to delete an existing number here if needed.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Donors', @level2type=N'COLUMN',@level2name=N'Envelope_No'
GO

