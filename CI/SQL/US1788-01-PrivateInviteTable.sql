USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Campaign_Private_Invitation]    Script Date: 9/1/2015 12:39:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cr_Campaign_Private_Invitation](
	[Private_Invitation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Pledge_Campaign_ID] [int] NOT NULL,
	[Email_Address] [dbo].[dp_Email] NOT NULL,
	[Recipient_Name] [nvarchar](75) NOT NULL,
	[Invitation_GUID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[_Invitation_Used] [bit] NOT NULL CONSTRAINT [DF__cr_Campai__Trip___25E8EAF9]  DEFAULT ((0)),
	[Domain_ID] [int] NOT NULL DEFAULT ((1)),
 CONSTRAINT [PK_cr_Campaign_Private_Invitation] PRIMARY KEY CLUSTERED
(
	[Private_Invitation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[cr_Campaign_Private_Invitation]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Private_Invitation_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] CHECK CONSTRAINT [FK_cr_Campaign_Private_Invitation_dp_Domains]
GO

ALTER TABLE [dbo].[cr_Campaign_Private_Invitation]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] CHECK CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]
GO
