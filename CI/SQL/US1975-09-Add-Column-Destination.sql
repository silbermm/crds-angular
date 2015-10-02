USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Pledge_Campaigns', N'COLUMN',N'Program_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pledge_Campaigns', @level2type=N'COLUMN',@level2name=N'Program_ID'

GO

IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Pledge_Campaigns', N'COLUMN',N'Event_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pledge_Campaigns', @level2type=N'COLUMN',@level2name=N'Event_ID'

GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Programs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] DROP CONSTRAINT [FK_Pledge_Campaigns_Programs]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Pledge_Campaign_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] DROP CONSTRAINT [FK_Pledge_Campaigns_Pledge_Campaign_Types]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Forms]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] DROP CONSTRAINT [FK_Pledge_Campaigns_Forms]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] DROP CONSTRAINT [FK_Pledge_Campaigns_Events]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] DROP CONSTRAINT [FK_Pledge_Campaigns_dp_Domains]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Programs_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Programs]'))
ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Form_Responses_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Form_Responses]'))
ALTER TABLE [dbo].[Form_Responses] DROP CONSTRAINT [FK_Form_Responses_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledges_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledges]'))
ALTER TABLE [dbo].[Pledges] DROP CONSTRAINT [FK_Pledges_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Statement_Settings_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Accounting_Companies]'))
ALTER TABLE [dbo].[Accounting_Companies] DROP CONSTRAINT [FK_Statement_Settings_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] DROP CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception] DROP CONSTRAINT [FK_cr_Campaign_Age_Exception_Pledge_Campaigns]
GO

/****** Done dropping FKs, build temp table, with new field ******/
CREATE TABLE [dbo].[tmp_Pledge_Campaigns](
	[Pledge_Campaign_ID] [int] IDENTITY(1,1) NOT NULL,
	[Campaign_Name] [nvarchar](50) NOT NULL,
	[Nickname] [nvarchar](50) NULL,
	[Pledge_Campaign_Type_ID] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Campaign_Goal] [money] NOT NULL,
	[Start_Date] [datetime] NOT NULL,
	[End_Date] [datetime] NULL,
	[Domain_ID] [int] NOT NULL,
	[Event_ID] [int] NULL,
	[Program_ID] [int] NULL,
	[Destination_ID] [int] NULL,
	[Registration_Details] [dbo].[dp_Separator] NULL,
	[Registration_Start] [datetime] NULL,
	[Registration_End] [datetime] NULL,
	[Maximum_Registrants] [int] NULL,
	[Youngest_Age_Allowed] [int] NULL,
	[Registration_Deposit] [money] NULL,
	[Fundraising_Goal] [money] NULL,
	[Registration_Form] [int] NULL,
	[Online_Pledge_Details] [dbo].[dp_Separator] NULL,
	[Allow_Online_Pledge] [bit] NOT NULL DEFAULT ((0)),
	[Online_Thank_You_Message] [nvarchar](255) NULL,
	[Pledge_Beyond_End_Date] [bit] NOT NULL DEFAULT ((0)),
	[Show_On_My_Pledges] [bit] NOT NULL DEFAULT ((0)),
	[__ExternalTripID] [int] NULL,
	[__ExternalFundID] [int] NULL,
CONSTRAINT [PK_tmp_Pledge_Campaigns] PRIMARY KEY CLUSTERED
(
	[Pledge_Campaign_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Copy data ******/
IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[Pledge_Campaigns])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_Pledge_Campaigns] ON;
        INSERT INTO [dbo].[tmp_Pledge_Campaigns]
			(
				[Pledge_Campaign_ID],
				[Campaign_Name],
				[Nickname],
				[Pledge_Campaign_Type_ID],
				[Description],
				[Campaign_Goal],
				[Start_Date],
				[End_Date],
				[Domain_ID],
				[Event_ID],
				[Program_ID],
				[Destination_ID],
				[Registration_Details],
				[Registration_Start],
				[Registration_End],
				[Maximum_Registrants],
				[Youngest_Age_Allowed],
				[Registration_Deposit],
				[Fundraising_Goal],
				[Registration_Form],
				[Online_Pledge_Details],
				[Allow_Online_Pledge],
				[Online_Thank_You_Message],
				[Pledge_Beyond_End_Date],
				[Show_On_My_Pledges],
				[__ExternalTripID],
				[__ExternalFundID]
			)
        SELECT
				[Pledge_Campaign_ID],
				[Campaign_Name],
				[Nickname],
				[Pledge_Campaign_Type_ID],
				[Description],
				[Campaign_Goal],
				[Start_Date],
				[End_Date],
				[Domain_ID],
				[Event_ID],
				[Program_ID],
        NULL, --new column Destination
				[Registration_Details],
				[Registration_Start],
				[Registration_End],
				[Maximum_Registrants],
				[Youngest_Age_Allowed],
				[Registration_Deposit],
				[Fundraising_Goal],
				[Registration_Form],
				[Online_Pledge_Details],
				[Allow_Online_Pledge],
				[Online_Thank_You_Message],
				[Pledge_Beyond_End_Date],
				[Show_On_My_Pledges],
				[__ExternalTripID],
				[__ExternalFundID]
        FROM     [dbo].[Pledge_Campaigns]
        ORDER BY [Pledge_Campaign_ID] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_Pledge_Campaigns] OFF;
    END


/****** Object:  Table [dbo].[Pledge_Campaigns]    Script Date: 8/12/2015 9:21:51 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]') AND type in (N'U'))
DROP TABLE [dbo].[Pledge_Campaigns]
GO

EXECUTE sp_rename N'[dbo].[tmp_Pledge_Campaigns]', N'Pledge_Campaigns';

EXECUTE sp_rename N'[dbo].[PK_tmp_Pledge_Campaigns]', N'PK_Pledge_Campaigns', N'OBJECT';

/****** Object:  Table [dbo].[Pledge_Campaigns]    Script Date: 8/12/2015 9:21:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_Events] FOREIGN KEY([Event_ID])
REFERENCES [dbo].[Events] ([Event_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Events]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_Events]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Forms]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_Forms] FOREIGN KEY([Registration_Form])
REFERENCES [dbo].[Forms] ([Form_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Forms]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_Forms]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Pledge_Campaign_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_Pledge_Campaign_Types] FOREIGN KEY([Pledge_Campaign_Type_ID])
REFERENCES [dbo].[Pledge_Campaign_Types] ([Pledge_Campaign_Type_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Pledge_Campaign_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_Pledge_Campaign_Types]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Programs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_Programs] FOREIGN KEY([Program_ID])
REFERENCES [dbo].[Programs] ([Program_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Programs]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_Programs]
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Pledge_Campaigns', N'COLUMN',N'Event_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link this pledge campaign to an event of type stewardship project that will be assigned to all donations that need to be matched to a pledge to this campaign.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pledge_Campaigns', @level2type=N'COLUMN',@level2name=N'Event_ID'
GO

IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Pledge_Campaigns', N'COLUMN',N'Program_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If this is a mission trip campaign the program listed here will be credited by default when a donation is made online to the pledge of one of the trip participants.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Pledge_Campaigns', @level2type=N'COLUMN',@level2name=N'Program_ID'
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Programs_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Programs]'))
ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Programs_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Programs]'))
ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Pledge_Campaigns]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Form_Responses_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Form_Responses]'))
ALTER TABLE [dbo].[Form_Responses]  WITH CHECK ADD  CONSTRAINT [FK_Form_Responses_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Form_Responses_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Form_Responses]'))
ALTER TABLE [dbo].[Form_Responses] CHECK CONSTRAINT [FK_Form_Responses_Pledge_Campaigns]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledges_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledges]'))
ALTER TABLE [dbo].[Pledges]  WITH CHECK ADD  CONSTRAINT [FK_Pledges_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledges_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledges]'))
ALTER TABLE [dbo].[Pledges] CHECK CONSTRAINT [FK_Pledges_Pledge_Campaigns]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Statement_Settings_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Accounting_Companies]'))
ALTER TABLE [dbo].[Accounting_Companies]  WITH CHECK ADD  CONSTRAINT [FK_Statement_Settings_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Statement_Settings_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[Accounting_Companies]'))
ALTER TABLE [dbo].[Accounting_Companies] CHECK CONSTRAINT [FK_Statement_Settings_Pledge_Campaigns]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Private_Invitation]'))
ALTER TABLE [dbo].[cr_Campaign_Private_Invitation] CHECK CONSTRAINT [FK_cr_Campaign_Private_Invitation_Pledge_Campaigns]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Age_Exception_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception] CHECK CONSTRAINT [FK_cr_Campaign_Age_Exception_Pledge_Campaigns]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Destinations]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns]  WITH CHECK ADD  CONSTRAINT [FK_Pledge_Campaigns_Destinations] FOREIGN KEY([Destination_ID])
REFERENCES [dbo].[cr_Destinations] ([Destination_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Pledge_Campaigns_Destinations]') AND parent_object_id = OBJECT_ID(N'[dbo].[Pledge_Campaigns]'))
ALTER TABLE [dbo].[Pledge_Campaigns] CHECK CONSTRAINT [FK_Pledge_Campaigns_Destinations]
GO
