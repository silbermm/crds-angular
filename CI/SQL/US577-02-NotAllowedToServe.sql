USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Serve_Restrictions]    Script Date: 11/12/2015 10:26:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Serve_Restrictions](
	[Serve_Restriction_ID] [int] IDENTITY(1,1) NOT NULL,
	[Contact_ID] [int] NOT NULL,
	[Restriction_Date] [date] NOT NULL,
	[Ministry_ID] [int] NOT NULL,
	[Serve_Restrictions_Reason_ID] [int] NOT NULL,
	[Comments] [nvarchar](2000),
 CONSTRAINT [PK_cr_Serve_Restriction] PRIMARY KEY CLUSTERED 
(
	[Serve_Restriction_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Domain_ID' AND Object_ID = Object_ID(N'cr_Serve_Restrictions'))
BEGIN
    ALTER TABLE dbo.cr_Serve_Restrictions ADD Domain_ID int NOT NULL CONSTRAINT DF_cr_Serve_Restrictions_Domain_ID DEFAULT 1
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Serve_Restrictions_Contacts]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions]'))
ALTER TABLE [dbo].[cr_Serve_Restrictions]  WITH CHECK ADD  CONSTRAINT [FK_cr_Serve_Restrictions_Contacts] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Serve_Restrictions_Ministries]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions]'))
ALTER TABLE [dbo].[cr_Serve_Restrictions]  WITH CHECK ADD  CONSTRAINT [FK_cr_Serve_Restrictions_Ministries] FOREIGN KEY([Ministry_ID])
REFERENCES [dbo].[Ministries] ([Ministry_ID])
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Serve_Restrictions_cr_Serve_Restrictions_Reasons]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions]'))
ALTER TABLE [dbo].[cr_Serve_Restrictions]  WITH CHECK ADD  CONSTRAINT [FK_cr_Serve_Restrictions_cr_Serve_Restrictions_Reasons] FOREIGN KEY([Serve_Restrictions_Reason_ID])
REFERENCES [dbo].[cr_Serve_Restrictions_Reasons] ([Serve_Restriction_Reason_ID])
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 541)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Sub_Pages]
           ([Sub_Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[On_Quick_Add]
           ,[Contact_ID_Field]
           ,[Display_Copy])
     VALUES
           (541
		   ,'Serve Restrictions'
           ,'Serve Restriction'
           ,292
           ,10
           ,'cr_Serve_Restrictions'
           ,'Serve_Restriction_ID'
           ,'Restriction_Date, Ministry_ID_Table.Ministry_Name, Serve_Restrictions_Reason_ID_Table.Serve_Restrictions_Reason, Comments'
           ,'Serve_Restrictions_Reason_ID_Table.Serve_Restrictions_Reason'
           ,'Contact_ID'
           ,1
           ,0
           ,'Contact_ID'
           ,0)
	SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Role_Sub_Pages] WHERE [Role_Sub_Page_ID] = 15484)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] ON
	INSERT INTO [dbo].[dp_Role_Sub_Pages]
           ([Role_Sub_Page_ID]
		   ,[Role_ID]
           ,[Sub_Page_ID]
           ,[Access_Level])
     VALUES
           (15484
		   ,2
           ,541
           ,3)
	SET IDENTITY_INSERT [dbo].[dp_Role_Sub_Pages] OFF
END
GO