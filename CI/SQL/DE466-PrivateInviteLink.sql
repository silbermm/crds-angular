USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] DROP CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] DROP CONSTRAINT [FK_cr_Campaign_Private_Invitation_dp_Domains]
GO

/****** Object:  Table [dbo].[cr_Campaign_Private_Invitation]    Script Date: 10/1/2015 8:43:18 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]') AND type in (N'U'))
DROP TABLE [dbo].[cr_Campaign_Private_Invitation]
GO

/****** Object:  Table [dbo].[cr_Campaign_Private_Invitation]    Script Date: 10/1/2015 8:43:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Campaign_Private_Invitation](
	[Private_Invitation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Pledge_Campaign_ID] [int] NOT NULL,
	[Email_Address] [dbo].[dp_Email] NOT NULL,
	[Recipient_Name] [nvarchar](75) NOT NULL,
	[Invitation_GUID] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Invitation_Used] [bit] NOT NULL CONSTRAINT [DF__cr_Campai__Trip___25E8EAF9]  DEFAULT ((0)),
	[Domain_ID] [int] NOT NULL DEFAULT ((1)),
 CONSTRAINT [PK_cr_Campaign_Private_Invitation] PRIMARY KEY CLUSTERED 
(
	[Private_Invitation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Private_Invitation_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] CHECK CONSTRAINT [FK_cr_Campaign_Private_Invitation_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] CHECK CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]
GO

UPDATE [dbo].[dp_Sub_Pages]
SET [Default_Field_List] = 'cr_Campaign_Private_Invitation.Email_Address, cr_Campaign_Private_Invitation.Recipient_Name,cr_Campaign_Private_Invitation.Invitation_Used as Used'
WHERE [Sub_Page_ID] = 534

UPDATE [dbo].[dp_Sub_Page_Views]
SET [Field_List] = 'cr_Campaign_Private_Invitation.[Private_Invitation_ID], Pledge_Campaign_ID_Table.[Pledge_Campaign_ID], cr_Campaign_Private_Invitation.[Invitation_GUID], cr_Campaign_Private_Invitation.[Invitation_Used], cr_Campaign_Private_Invitation.[Email_Address]'
WHERE [Sub_Page_View_ID] = 101

UPDATE [dbo].[dp_Pages]
SET [Default_Field_List] = 'cr_Campaign_Private_Invitation.Email_Address, cr_Campaign_Private_Invitation.Recipient_Name, cr_Campaign_Private_Invitation.Invitation_Used as Used'
WHERE [Page_ID] = 515

UPDATE [dbo].[dp_Communications]
SET [Body] = '<div>[ParticipantName],</div><div><br /></div><div>Please use the link below to complete the [TripTitle] application.  The email address you register with on crossroads.net must match the email address that received this message.</div><div><br /></div><div>http://[BaseUrl]/trips/[PledgeCampaignID]?invite=[InviteGUID]</div>'
WHERE [Communication_ID] = 12302 

SET ANSI_PADDING OFF
GO