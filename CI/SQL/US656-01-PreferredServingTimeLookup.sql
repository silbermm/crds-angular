USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = 'MinistryPlatform' AND TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'cr_Preferred_Serve_Time')
BEGIN 
	CREATE TABLE [dbo].[cr_Preferred_Serve_Time](
		[Preferred_Serving_Time_ID] INT IDENTITY(1,1) NOT NULL,
		[Preferred_Serve_Time] NVARCHAR(50) NOT NULL,
		[Congregation_ID] INT NOT NULL,
		[Domain_ID] INT NOT NULL DEFAULT (1)
	CONSTRAINT [PK_Preferred_Serve_Time] PRIMARY KEY CLUSTERED 
	(
		[Preferred_Serving_Time_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Preferred_Serve_Time_Congregation]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Preferred_Serve_Time]'))
ALTER TABLE [dbo].[cr_Preferred_Serve_Time]  WITH CHECK ADD  CONSTRAINT [FK_cr_Preferred_Serve_Time_Congregation] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Preferred_Serve_Time_Congregation]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Preferred_Serve_Time]'))
ALTER TABLE [dbo].[cr_Preferred_Serve_Time] CHECK CONSTRAINT FK_cr_Preferred_Serve_Time_Congregation
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Preferred_Serve_Time_Domain]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Preferred_Serve_Time]'))
ALTER TABLE [dbo].[cr_Preferred_Serve_Time]  WITH CHECK ADD  CONSTRAINT [FK_cr_Preferred_Serve_Time_Domain] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Preferred_Serve_Time_Congregation]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Preferred_Serve_Time]'))
ALTER TABLE [dbo].[cr_Preferred_Serve_Time] CHECK CONSTRAINT FK_cr_Preferred_Serve_Time_Domain
GO

/* Add Lookup page for this table */
IF NOT EXISTS(SELECT * FROM [dbo].[dp_Pages] WHERE Page_ID = 539)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Pages] ON

INSERT INTO [dbo].[dp_Pages]
			([Page_ID]
           ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Display_Copy])
     VALUES
			(539,
           'Preferred Serving Time',
           'Preferred Serving Time',
           'Preferred Serving Time',
           295,
           'cr_Preferred_Serve_Time',
           'Preferred_Serving_Time_ID',
           NULL,
           'cr_Preferred_Serve_Time.Preferred_Serve_Time, Congregation_ID_Table.[Congregation_Name]',
           'cr_Preferred_Serve_Time.Preferred_Serve_Time',
           0)

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF

INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([Page_ID]
           ,[Page_Section_ID])
     VALUES
           (539, 4)

INSERT INTO [dbo].[dp_Role_Pages]
           ([Role_ID]
           ,[Page_ID]
           ,[Access_Level]
           ,[Scope_All]
           ,[Approver]
           ,[File_Attacher]
           ,[Data_Importer]
           ,[Data_Exporter]
           ,[Secure_Records]
           ,[Allow_Comments]
           ,[Quick_Add])
     VALUES
           (2,539,3,0,0,0,0,0,0,0,0)
END