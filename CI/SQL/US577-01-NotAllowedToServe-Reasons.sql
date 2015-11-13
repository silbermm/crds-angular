USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Serve_Restrictions_Reasons]    Script Date: 11/12/2015 10:26:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Serve_Restrictions_Reasons]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Serve_Restrictions_Reasons](
	[Serve_Restriction_Reason_ID] [int] IDENTITY(1,1) NOT NULL,
	[Serve_Restrictions_Reason] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_cr_Serve_Restrictions_Reason] PRIMARY KEY CLUSTERED 
(
	[Serve_Restriction_Reason_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Pages] where [Page_ID] = 527)
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
           (527
		   ,'Serve Restriction Reasons'
           ,'Serve Restriction Reason'
           ,'Reasons why people are restricted from serving'
           ,475
           ,'cr_Serve_Restrictions_Reasons'
           ,'Serve_Restriction_Reason_ID'
           ,0
           ,'Serve_Restrictions_Reason'
           ,'Serve_Restrictions_Reason'
           ,1)
	SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Page_Section_Pages] WHERE [dp_Page_Section_Page_ID] = 262)
BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Page_Section_Pages] ON
	INSERT INTO [dbo].[dp_Page_Section_Pages]
           ([dp_Page_Section_Page_ID]
		   ,[Page_ID]
           ,[Page_Section_ID])
     VALUES
           (262
		   ,527
           ,4)
	SET IDENTITY_INSERT [dbo].[dp_Page_Section_Pages] OFF
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[cr_Serve_Restrictions_Reasons] WHERE [Serve_Restriction_Reason_ID] = 1)
BEGIN
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] ON
	INSERT INTO [dbo].[cr_Serve_Restrictions_Reasons]
           ([Serve_Restriction_Reason_ID]
		   ,[Serve_Restrictions_Reason])
     VALUES
           (1
		   ,'Not Permitted to Serve')
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] OFF
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[cr_Serve_Restrictions_Reasons] WHERE [Serve_Restriction_Reason_ID] = 2)
BEGIN
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] ON
	INSERT INTO [dbo].[cr_Serve_Restrictions_Reasons]
           ([Serve_Restriction_Reason_ID]
		   ,[Serve_Restrictions_Reason])
     VALUES
           (2
		   ,'Person of Interest')
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] OFF
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[cr_Serve_Restrictions_Reasons] WHERE [Serve_Restriction_Reason_ID] = 3)
BEGIN
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] ON
	INSERT INTO [dbo].[cr_Serve_Restrictions_Reasons]
           ([Serve_Restriction_Reason_ID]
		   ,[Serve_Restrictions_Reason])
     VALUES
           (3
		   ,'Restriction Removed')
	SET IDENTITY_INSERT [dbo].[cr_Serve_Restrictions_Reasons] OFF 
END
GO