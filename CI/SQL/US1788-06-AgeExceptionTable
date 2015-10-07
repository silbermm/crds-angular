USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Campaign_Age_Exception](
	[Age_Exception_ID] [int] IDENTITY(1,1) NOT NULL,
	[Pledge_Campaign_ID] [int] NOT NULL,
	[Contact_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL DEFAULT ((1))
 CONSTRAINT [PK_cr_Campaign_Age_Exception] PRIMARY KEY CLUSTERED
(
	[Age_Exception_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Age_Exception_Pledge_Campaigns] FOREIGN KEY([Pledge_Campaign_ID])
REFERENCES [dbo].[Pledge_Campaigns] ([Pledge_Campaign_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Pledge_Campaigns]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception] CHECK CONSTRAINT [FK_cr_Campaign_Age_Exception_Pledge_Campaigns]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Age_Exception_Contacts] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception] CHECK CONSTRAINT [FK_cr_Campaign_Age_Exception_Contacts]
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception]  WITH CHECK ADD  CONSTRAINT [FK_cr_Campaign_Age_Exception_dp_Domains] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Campaign_Age_Exception_dp_Domains]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Campaign_Age_Exception]'))
ALTER TABLE [dbo].[cr_Campaign_Age_Exception] CHECK CONSTRAINT [FK_cr_Campaign_Age_Exception_dp_Domains]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 535)
SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           (535
		   ,'Age Exceptions'
           ,'Age Exception'
           ,514
           ,4
           ,'cr_Campaign_Age_Exception'
           ,'cr_Campaign_Age_Exception.Contact_ID, Contact_ID_Table.[Display_Name]'
           ,'cr_Campaign_Age_Exception.Contact_ID'
           ,'Pledge_Campaign_ID'
           ,1
           ,0)

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
GO
