USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[Donation_Status_Mapping]    Script Date: 7/1/2015 9:41:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Donation_Status_Mapping](
	[Domain_ID] [int] NOT NULL CONSTRAINT [DF_Donation_Status_Mapping_Domain_ID]  DEFAULT ((1)),
	[Donation_Status_Mapping_ID] [int] IDENTITY(1,1) NOT NULL,
	[Donation_Status_Date] [datetime] NOT NULL,
	[Donation_ID] [int] NOT NULL,
	[Donation_Status_ID] [int] NOT NULL,
	[Donation_Status_Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Donation_Status_Mapping] PRIMARY KEY CLUSTERED 
(
	[Donation_Status_Mapping_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Donation_Status_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Status_Mapping_Domain_ID] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Donation_Status_Mapping] CHECK CONSTRAINT [FK_Donation_Status_Mapping_Domain_ID]
GO

ALTER TABLE [dbo].[Donation_Status_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Status_Mapping_Donation_Status] FOREIGN KEY([Donation_Status_ID])
REFERENCES [dbo].[Donation_Status] ([Donation_Status_ID])
GO

ALTER TABLE [dbo].[Donation_Status_Mapping] CHECK CONSTRAINT [FK_Donation_Status_Mapping_Donation_Status]
GO

ALTER TABLE [dbo].[Donation_Status_Mapping]  WITH CHECK ADD  CONSTRAINT [FK_Donation_Status_Mapping_Donations] FOREIGN KEY([Donation_ID])
REFERENCES [dbo].[Donations] ([Donation_ID])
GO

ALTER TABLE [dbo].[Donation_Status_Mapping] CHECK CONSTRAINT [FK_Donation_Status_Mapping_Donations]
GO


