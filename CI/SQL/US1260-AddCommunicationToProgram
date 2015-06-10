USE [MinistryPlatform]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Account_Number'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Pledge_Campaign_ID'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Online_Sort_Order'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Allow_Online_Giving'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Statement_Header_ID'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Statement_Title'

GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Tax_Deductible_Donations'

GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Statement_Headers]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Program_Types]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Priorities]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Pledge_Campaigns]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Ministries]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Groups]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Events]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_dp_Domains]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_dp_Communications]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Contacts]
GO

ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_Programs_Congregations]
GO

/****** Object:  Table [dbo].[Programs]    Script Date: 6/5/2015 3:48:36 PM ******/
DROP TABLE [dbo].[Programs]
GO

/****** Object:  Table [dbo].[Programs]    Script Date: 6/5/2015 3:48:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Programs](
  [Program_ID] [int] IDENTITY(1,1) NOT NULL,
  [Program_Name] [nvarchar](130) NOT NULL,
  [Congregation_ID] [int] NOT NULL,
  [Ministry_ID] [int] NOT NULL,
  [Start_Date] [datetime] NOT NULL,
  [End_Date] [datetime] NULL,
  [Program_Type_ID] [int] NULL,
  [Leadership_Team] [int] NULL,
  [Primary_Contact] [int] NOT NULL,
  [Priority_ID] [int] NULL,
  [On_Connection_Card] [bit] NULL,
  [Stewardship_Information] [dbo].[dp_Separator] NULL,
  [Tax_Deductible_Donations] [bit] NOT NULL CONSTRAINT [DF_Programs_Tax_Deductible_Donations]  DEFAULT ((0)),
  [Statement_Title] [nvarchar](50) NOT NULL,
  [Statement_Header_ID] [int] NOT NULL,
  [Allow_Online_Giving] [bit] NOT NULL CONSTRAINT [DF_Programs_Available_Online]  DEFAULT ((0)),
  [Online_Sort_Order] [smallint] NULL,
  [Pledge_Campaign_ID] [int] NULL,
  [Account_Number] [nvarchar](25) NULL,
  [Default_Target_Event] [int] NULL,
  [On_Donation_Batch_Tool] [bit] NOT NULL CONSTRAINT [DF__Programs__On_Don__1DB135A6]  DEFAULT ((0)),
  [Domain_ID] [int] NOT NULL,
  [Available_Online] [bit] NULL,
  [__ExternalFundID] [int] NULL,
  [Communication_ID] [int] NULL,
 CONSTRAINT [PK_Programs] PRIMARY KEY CLUSTERED 
(
  [Program_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Congregations] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Congregations]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Contacts] FOREIGN KEY([Primary_Contact])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Contacts]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_dp_Communications] FOREIGN KEY([Communication_ID])
REFERENCES [dbo].[dp_Communications] ([Communication_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_dp_Communications]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_dp_Domains]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Events] FOREIGN KEY([Default_Target_Event])
REFERENCES [dbo].[Events] ([Event_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Events]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Groups] FOREIGN KEY([Leadership_Team])
REFERENCES [dbo].[Groups] ([Group_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Groups]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Ministries] FOREIGN KEY([Ministry_ID])
REFERENCES [dbo].[Ministries] ([Ministry_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Ministries]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Pledge_Campaigns]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Priorities] FOREIGN KEY([Priority_ID])
REFERENCES [dbo].[Priorities] ([Priority_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Priorities]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Program_Types] FOREIGN KEY([Program_Type_ID])
REFERENCES [dbo].[Program_Types] ([Program_Type_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Program_Types]
GO

ALTER TABLE [dbo].[Programs]  WITH CHECK ADD  CONSTRAINT [FK_Programs_Statement_Headers] FOREIGN KEY([Statement_Header_ID])
REFERENCES [dbo].[Statement_Headers] ([Statement_Header_ID])
GO

ALTER TABLE [dbo].[Programs] CHECK CONSTRAINT [FK_Programs_Statement_Headers]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Donations can be distributed to any program, but only donations to programs with a yes in this field will appear on a donors statement.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Tax_Deductible_Donations'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This name will appear on the online giving page and on the donor statement.  Keep it short!' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Statement_Title'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates the column this program will be listed under for donor statemenets and other donation reports.  If empty donations to this program will be under the other column.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Statement_Header_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Selecting Yes will allow donors to give online to this program IF it is of type stewardship.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Allow_Online_Giving'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This sort order will ensure that the online designations for giving appear in the order desired.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Online_Sort_Order'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The campaign currently associated with donations to this program' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Programs', @level2type=N'COLUMN',@level2name=N'Pledge_Campaign_ID'
